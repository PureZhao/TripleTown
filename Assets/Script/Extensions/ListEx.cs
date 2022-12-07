using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;

public static class ListEx
{
    public static T Back<T>(this List<T> list)
    {
        if(list == null && list.Count <= 0)
        {
            return default;
        }
        return list[list.Count - 1];
    }

    public static T RandomElect<T>(this List<T> list)
    {
        if(list == null || list.Count == 0)
        {
            throw new NullReferenceException("List Should not be Null When Random Elect");
        }
        int index = UnityEngine.Random.Range(0, list.Count);
        return list[index];
    }

    public static void Shuffle<T>(this List<T> list)
    {
        int len = list.Count;
        for(int i = len - 1; i >= 0; i--)
        {
            int index = UnityEngine.Random.Range(0, len);
            T temp = list[i];
            list[i] = list[index];
            list[index] = temp;
        }
    }

    public static void Add<T>(this List<T> list, int count, T val)
    {
        while(count > 0)
        {
            list.Add(val);
            count--;
        }
    }

    public static List<object> ConvertToObjectList(params object[] objs)
    {
        return objs.ToList();
    }
}
