using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCore
{
    [LuaCallCSharp]
    public class LuaBehaviour : MonoBehaviour
    {
        public interface IObjWrap
        {
            object GetObj();
            void SetObj(object obj);
        }

        public class ObjWrap<T> : IObjWrap
        {
            public T obj;

            public object GetObj()
            {
                if (obj == null)
                {
                    if(typeof(T) == typeof(string))
                        return "";
                    return default;
                }
                return obj;
            }

            public void SetObj(object obj)
            {
                this.obj = (T)obj;
            }
        }

        public static string ValueToJson(object value)
        {
            Type type = value.GetType();
            Type wrapType = typeof(ObjWrap<>).MakeGenericType(type);
            var target = Activator.CreateInstance(wrapType);
            ((IObjWrap)target).SetObj(value);
            return JsonUtility.ToJson(target);
        }

        public static object JsonToValue(string json, Type type)
        {
            if (wrapTypeDict.ContainsKey(type))
            {
                Type wrapType = wrapTypeDict[type];
                var target = JsonUtility.FromJson(json, wrapType);
                return ((IObjWrap)target).GetObj();
            }
            else if (type.IsValueType())
            {
                //Debug.LogError($"msut register {type.FullName} in wrapTypeDict!");
                Debug.LogError($"msut register {type.FullName} in wrapTypeDict, because it is value type!");
                return null;
            }
            else
            {
                Type wrapType = typeof(ObjWrap<>).MakeGenericType(type);
                wrapTypeDict.Add(type, wrapType);
                var target = JsonUtility.FromJson(json, wrapType);
                return ((IObjWrap)target).GetObj();
            }
        }

        //因为值类型泛型的生成在IL2CPP下会有JIT异常，所以值类型必须要注册
        private static Dictionary<Type, Type> wrapTypeDict = new Dictionary<Type, Type>()
        {
            {typeof(Int32), typeof(ObjWrap<Int32>)},
            {typeof(Int64), typeof(ObjWrap<Int64>)},
            {typeof(Single), typeof(ObjWrap<Single>)},
            {typeof(Double), typeof(ObjWrap<Double>)},
            {typeof(Boolean), typeof(ObjWrap<Boolean>)},
            {typeof(string), typeof(ObjWrap<string>)},
            {typeof(Vector2), typeof(ObjWrap<Vector2>)},
            {typeof(Vector3), typeof(ObjWrap<Vector3>)},
            {typeof(Vector4), typeof(ObjWrap<Vector4>)},
            {typeof(Color), typeof(ObjWrap<Color>)},
            {typeof(Rect), typeof(ObjWrap<Rect>)},
            {typeof(Bounds), typeof(ObjWrap<Bounds>)},

            {typeof(List<int>), typeof(ObjWrap<List<int>>)},
            {typeof(List<string>), typeof(ObjWrap<List<string>>)},
            {typeof(List<Vector3>), typeof(ObjWrap<List<Vector3>>)},
            {typeof(List<Color>), typeof(ObjWrap<List<Color>>)},
            {typeof(ElementType), typeof(ObjWrap<ElementType>)},
            {typeof(ToolType), typeof(ObjWrap<ToolType>)},

        };
        [Serializable]
        public class ObjectWrap
        {
            public int index;
            public string name;
            public UnityEngine.Object obj;
            public string typeName;
            public string assembly;
        }
        [Serializable]
        public class ValueWrap
        {
            public int index;
            public string name;
            public string jsonStr;
            public string typeName;
            public string assembly;
        }

        public void Rebuild()
        {
#if UNITY_EDITOR
            string assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            if (string.IsNullOrEmpty(assetPath)) return;
            if (File.Exists(assetPath))
            {
                File.Delete(assetPath);
            }
            if(File.Exists(assetPath + ".meta"))
            {
                File.Delete(assetPath + ".meta");
            }
            PrefabUtility.SaveAsPrefabAsset(gameObject, assetPath, out bool s);
#endif
        }

#if UNITY_EDITOR
        public DefaultAsset luaScript;
#endif
        public string requirePath;

        //internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1; //1 second 
        
        private LuaFunction luaUpdate;
        private LuaFunction luaOnDestroy;

        [SerializeField]
        public List<ObjectWrap> objects = new List<ObjectWrap>();
        [SerializeField]
        public List<ValueWrap> values = new List<ValueWrap>();
        private LuaTable luaClass;
        private LuaBehaviourMouse mouseBehaviour;
        public LuaTable GetLuaClass()
        {
            return luaClass;
        }

        void Awake()
        {
            // 将需要提前注册的变量放到这个表
            LuaTable injections = ProjectLuaEnv.Instance.NewTable();
            // 注入值类型 必须在wrapTypeDict中有
            for (int i = 0; i < values.Count; i++)
            {
                injections.Set(values[i].name, JsonToValue(values[i].jsonStr, Type.GetType(values[i].typeName)));
            }
            // 注入其他类型
            for (int i = 0; i < objects.Count; i++)
            {
                injections.Set(objects[i].name, objects[i].obj);
            }
            // 将自己的一些东西注册进去
            injections.Set("host", this);
            injections.Set("gameObject", gameObject);
            injections.Set("transform", transform);
            
            string cmd = string.Format("require('Core.Global'); local t = require('{0}'); return t;", requirePath);
            LuaTable table = (LuaTable)ProjectLuaEnv.Instance.DoString(cmd)[0];
            LuaFunction newFunc = table.Get<LuaFunction>("NewFromCS");
            // 初始化类
            luaClass = (LuaTable)newFunc.Call(injections)[0];
            LuaFunction luaAwake = luaClass.Get<LuaFunction>("__init");

            luaClass.Get("Delete", out luaOnDestroy);
            luaClass.Get("Update", out luaUpdate);
            if(TryGetComponent(out mouseBehaviour))
            {
                mouseBehaviour.BindEvent(luaClass);
            }

            luaAwake?.Call(luaClass);
        }

        
        // Update is called once per frame
        void Update()
        {
            luaUpdate?.Call(luaClass);
            if (Time.time - LuaBehaviour.lastGCTime > GCInterval)
            {
                ProjectLuaEnv.Instance.Tick();
                LuaBehaviour.lastGCTime = Time.time;
            }
        }

        void OnDestroy()
        {
            StopAllCoroutines();
            luaOnDestroy?.Call(luaClass);

            luaOnDestroy = null;
            luaUpdate = null;
        }

        public void ActivateMouseEvent(bool state)
        {
            mouseBehaviour?.ActivateEvent(state);
        }
    }
}
