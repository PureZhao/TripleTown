using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{

    public class GameObjectPool : MonoBehaviour
    {
        private static GameObjectPool instane;
        public static GameObjectPool Instane { get => instane; }
        Dictionary<string, List<GameObject>> pool;

        public void PushInPool(GameObject obj)
        {
            AssetFlag flag = obj.GetComponent<AssetFlag>();
            if(flag != null)
            {
                string key = flag.Path;
                if (!pool.ContainsKey(key))
                {
                    pool.Add(key, new List<GameObject>());
                }
                pool[key].Add(obj);
                obj.transform.parent = transform;
            }
            else
            {
                Destroy(flag);
            }
        }

        public GameObject PushOutPull(string key)
        {
            if (pool.ContainsKey(key))
            {
                List<GameObject> objs = pool[key];
                int count = objs.Count;
                if(objs.Count > 0)
                {
                    GameObject target = objs.Back();
                    objs.RemoveAt(count - 1);
                    return target;
                }
            }
            return null;
        }

        public void Clear()
        {
            foreach(List<GameObject> objs in pool.Values)
            {
                for(int i = 0; i < objs.Count; i++)
                {
                    if(objs != null)
                    {
                        Destroy(objs[i]);
                    }
                }
            }
            pool.Clear();
        }

        private void Awake()
        {
            if(instane == null)
            {
                instane = this;
            }
            else
            {
                Destroy(this);
            }
            DontDestroyOnLoad(gameObject);
            pool = new Dictionary<string, List<GameObject>>();
        }

        private void OnDestroy()
        {
            Clear();
            pool = null;
        }
    }

}