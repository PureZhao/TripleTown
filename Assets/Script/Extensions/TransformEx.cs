using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class TransformEx
{
    public static T GetOrAddComponent<T>(this Transform trans, Action<T> onGet = null) where T : Component
    {
        T comp = trans.GetComponent<T>();
        if(comp == null)
        {
            comp = trans.gameObject.AddComponent<T>();
        }
        onGet?.Invoke(comp);
        return comp;
    }

    public static Transform GetOrAddChild(this Transform trans, string childName)
    {
        Transform child = trans.Find(childName);
        if (child == null)
        {
            GameObject go = new GameObject(childName);
            go.transform.parent = trans;
            child = go.transform;
        }
        return child;
    }

    public static void DeleteComponent<T>(this Transform trans) where T : Component
    {
        T comp = trans.GetComponent<T>();
        if(comp != null)
        {
            Component.DestroyImmediate(comp);
        }
    }

    public static void DeleteComponents<T>(this Transform trans) where T : Component
    {
        T[] comps = trans.GetComponents<T>();
        if (comps.Length > 0)
        {
            for(int i = 0; i < comps.Length; i++)
            {
                Component.DestroyImmediate(comps[i]);
            }
        }
    }

    public static void ResetPRS(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

}
