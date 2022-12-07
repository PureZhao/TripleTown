using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StringEx
{
    public static string Remove(this string text, string wannaRemove)
    {
        if (string.IsNullOrEmpty(wannaRemove)) return text;
        if (text.Length < wannaRemove.Length) return text;
        string clone = text.Clone().ToString();
        int index = clone.IndexOf(wannaRemove);
        int len = wannaRemove.Length;
        while (index != -1)
        {
            clone = clone.Remove(index, len);
            index = clone.IndexOf(wannaRemove);
        }
        return clone;
    }

    public static List<string> Split(this string text, string spliter)
    {
        List<string> res = new List<string>(text.Split(spliter.ToCharArray()));
        while (res.Contains(""))
        {
            res.Remove("");
        }
        return res;
    }

    static int[] GetPrefixTable(string pattern)
    {
        int[] prefix = new int[pattern.Length];
        prefix[0] = 0;
        int len = 0;
        int i = 1;
        int m = pattern.Length;
        while(i < m)
        {
            if (pattern[i] == pattern[len])
            {
                len++;
                prefix[i] = len;
                i++;
            }
            else
            {
                if(len > 0)
                {
                    len = prefix[len - 1];
                }
                else
                {
                    prefix[i] = len;
                    i++;
                }
            }
        }
        for(int j = prefix.Length - 1; j > 0; j--)
        {
            prefix[j] = prefix[j - 1];
        }
        prefix[0] = -1;
        return prefix;
    }
    public static int KmpIndexOf(this string text, string pattern)
    {
        if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text)) return -1;
        int m = text.Length;
        int n = pattern.Length;
        if (m < n) return -1;
        int[] prefix = GetPrefixTable(pattern);
        int i = 0, j = 0;
        while(i < m)
        {
            if(j == n - 1 && text[i] == pattern[j])
            {
                return i - j;
            }
            if (text[i] == pattern[j])
            {
                i++;
                j++;
            }
            else
            {
                j = prefix[j];
                if(j == -1)
                {
                    i++;
                    j++;
                }
            }
        }
        return -1;
    }
    public static int[] KmpIndicesOf(this string text, string pattern)
    {
        if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text)) return null;
        int m = text.Length;
        int n = pattern.Length;
        if (m < n) return null;
        List<int> indices = new List<int>();
        int[] prefix = GetPrefixTable(pattern);
        int i = 0, j = 0;
        while (i < m)
        {
            if (j == n - 1 && text[i] == pattern[j])
            {
                indices.Add(i - j);
                j = prefix[j];
            }
            if (text[i] == pattern[j])
            {
                i++;
                j++;
            }
            else
            {
                j = prefix[j];
                if (j == -1)
                {
                    i++;
                    j++;
                }
            }
        }
        return indices.ToArray();
    }

    public static byte[] ToByteArray(this string str)
    {
        if(str == string.Empty)
        {
            return null;
        }
        byte[] bytes = new byte[str.Length];
        for(int i = 0;i < str.Length; i++)
        {
            bytes[i] = Convert.ToByte(str[i]);
        }
        return bytes;
    }

}
