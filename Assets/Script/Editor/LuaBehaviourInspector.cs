using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using System;
using XLua;

[CustomEditor(typeof(GameCore.LuaBehaviour))]
public class LuaBehaviourInspector : Editor
{

    private GameCore.LuaBehaviour behaviour;
    static Dictionary<Type, Type> objWrapEditorDict = new Dictionary<Type, Type>();
    static ModuleBuilder editorModule;
    static LuaBehaviourInspector()
    {
        AppDomain myDomain = Thread.GetDomain();
        AssemblyName myAsmName = new AssemblyName();
        myAsmName.Name = "LuaBehaviourEditor";
        AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);
        editorModule = myAsmBuilder.DefineDynamicModule("LuaBehaviourEditorModule", "LuaBehaviourEditor.dll");
    }

    static Type GetWrapType(Type objType)
    {
        if (objWrapEditorDict.ContainsKey(objType))
        {
            return objWrapEditorDict[objType];
        }
        TypeBuilder wrapTypeBld = editorModule.DefineType("wrap" + objType.FullName, TypeAttributes.Public, typeof(ScriptableObject));
        FieldBuilder objField = wrapTypeBld.DefineField("obj", objType, FieldAttributes.Public);
        Type wrapType = wrapTypeBld.CreateType();
        objWrapEditorDict.Add(objType, wrapType);
        return wrapType;
    }

    void GetPreObject(ref GameCore.LuaBehaviour.ObjectWrap cur, ref List<GameCore.LuaBehaviour.ObjectWrap> objs)
    {
        if(objs != null)
        {
            for(int i = 0; i < objs.Count; i++)
            {
                if (objs[i].name == cur.name && objs[i].typeName == cur.typeName && objs[i].assembly == cur.assembly)
                {
                    cur.obj = objs[i].obj;
                }
            }
        }
    }

    void GetPreValue(ref GameCore.LuaBehaviour.ValueWrap cur, ref List<GameCore.LuaBehaviour.ValueWrap> vals)
    {
        if (vals != null)
        {
            for (int i = 0; i < vals.Count; i++)
            {
                if (vals[i].name == cur.name && vals[i].typeName == cur.typeName && vals[i].assembly == cur.assembly)
                {
                    cur.jsonStr = vals[i].jsonStr;
                }
            }
        }
    }

    void OnEnable()
    {
        behaviour = target as GameCore.LuaBehaviour;
    }

    void LoadLua()
    {
        List<GameCore.LuaBehaviour.ObjectWrap> lastObjects = behaviour.objects;
        List<GameCore.LuaBehaviour.ValueWrap> lastValues = behaviour.values;

        behaviour.objects = new List<GameCore.LuaBehaviour.ObjectWrap>();
        behaviour.values = new List<GameCore.LuaBehaviour.ValueWrap>();

        if (behaviour.luaScript != null)
        {
            string path = AssetDatabase.GetAssetPath(behaviour.luaScript).Replace('\\', '/');
            string luaDir = "Assets/Script/Lua/";
            string requirePath = path.Substring(luaDir.Length).Remove(".lua").Replace('/', '.');
            string cmd = string.Format("local t = require('{0}'); return t;", requirePath);
            behaviour.requirePath = requirePath;
            ProjectLuaEnv.Instance.Dispose();  // 直接销毁 不然还是用的原来的table 属性不会变
            object[] r = ProjectLuaEnv.Instance.DoString(cmd);

            LuaTable table = (LuaTable)r[0];
            LuaFunction action = table.Get<LuaFunction>("Define");
            action?.Call(table);   // C# 还是按照Lua的方式访问函数
            LuaTable defineList = table.Get<LuaTable>("_DefineList");
            for(int i = 1; i <= defineList.Length; i++)
            {
                LuaTable t = defineList.Get<object, LuaTable>(i);
                string name = t.Get<string>("name");
                Type type = t.Get<Type>("type");
                if (type.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    GameCore.LuaBehaviour.ObjectWrap obj = new GameCore.LuaBehaviour.ObjectWrap()
                    {
                        index = i,
                        name = name,
                        typeName = type.FullName,
                        assembly = Assembly.GetAssembly(type).GetName().Name,
                    };
                    GetPreObject(ref obj, ref lastObjects);
                    behaviour.objects.Add(obj);
                }
                else
                {
                    GameCore.LuaBehaviour.ValueWrap val = new GameCore.LuaBehaviour.ValueWrap()
                    {
                        index = i,
                        name = name,
                        jsonStr = "{}",
                        typeName = type.FullName,
                        assembly = Assembly.GetAssembly(type).GetName().Name,
                    };
                    GetPreValue(ref val, ref lastValues);
                    behaviour.values.Add(val);
                }
                
            }
            table.Dispose();
            defineList.Dispose();
            action.Dispose();
            ProjectLuaEnv.Instance.Dispose();
        }
        behaviour.Rebuild();
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        //绘制Lua路径
        if(GUILayout.Button("Load Lua"))
        {
            LoadLua();
        }
        if(behaviour.transform.parent == null)
        {
            if (GUILayout.Button("Save"))
            {
                behaviour.Rebuild();
            }
        }
        behaviour.luaScript = EditorGUILayout.ObjectField("Lua Script", behaviour.luaScript, typeof(DefaultAsset)) as DefaultAsset;
        EditorGUILayout.LabelField("Require Path", behaviour.requirePath);

        int count = behaviour.objects.Count + behaviour.values.Count;

        for(int i = 1; i <= count; i++)
        {
            for(int j = 0; j < behaviour.objects.Count; j++)
            {
                GameCore.LuaBehaviour.ObjectWrap wrap = behaviour.objects[j];
                if (wrap.index == i)
                {
                    wrap.obj = EditorGUILayout.ObjectField(wrap.name, wrap.obj, Assembly.Load(wrap.assembly).GetType(wrap.typeName));
                }
            }
            for (int k = 0; k < behaviour.values.Count; k++)
            {
                GameCore.LuaBehaviour.ValueWrap wrap = behaviour.values[k];
                if(wrap.index == i)
                {
                    Type t = Assembly.Load(wrap.assembly).GetType(wrap.typeName);
                    var editorType = GetWrapType(t);
                    var objField = editorType.GetField("obj");
                    var target = CreateInstance(editorType);
                    var value = GameCore.LuaBehaviour.JsonToValue(wrap.jsonStr, t);
                    objField.SetValue(target, value);
                    var so = new SerializedObject(target);
                    EditorGUILayout.PropertyField(so.FindProperty("obj"), new GUIContent(wrap.name));
                    so.ApplyModifiedProperties();
                    wrap.jsonStr = GameCore.LuaBehaviour.ValueToJson(objField.GetValue(target));
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
