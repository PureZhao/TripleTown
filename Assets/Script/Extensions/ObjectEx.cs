using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public static class ObjectEx
{
    public static bool IsNull(this UnityEngine.Object obj)
    {
        return obj == null;
    }
}
