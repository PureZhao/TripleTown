using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

[LuaCallCSharp]
public static class GameObjectEx
{
    public static T GetOrAddComponent<T>(this GameObject obj, Action<T> onGet = null) where T : Component
    {
        T comp = obj.GetComponent<T>();
        if (comp == null)
        {
            comp = obj.AddComponent<T>();
        }
        onGet?.Invoke(comp);
        return comp;
    }

    public static GameObject GetOrAddChild(this GameObject obj, string childName)
    {
        Transform child = obj.transform.Find(childName);
        if (child == null)
        {
            GameObject go = new GameObject(childName);
            go.transform.parent = obj.transform;
            child = go.transform;
        }
        return child.gameObject;
    }

    public static void DeleteComponent<T>(this GameObject obj) where T : Component
    {
        T comp = obj.GetComponent<T>();
        if (comp != null)
        {
            Component.DestroyImmediate(comp);
        }
    }

    public static void DeleteComponents<T>(this GameObject obj) where T : Component
    {
        T[] comps = obj.GetComponents<T>();
        if (comps.Length > 0)
        {
            for (int i = 0; i < comps.Length; i++)
            {
                Component.DestroyImmediate(comps[i]);
            }
        }
    }


    public static void SetParent(this GameObject obj, Transform parent = null)
    {
        obj.transform.parent = parent;
    }

}
