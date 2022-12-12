using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public static class XLuaRequireType
{
    [LuaCallCSharp]
    public static List<Type> LuaCallCharp = new List<Type>()
    {
        typeof(WaitForSeconds),
        typeof(WaitUntil),
        typeof(Rect),
        typeof(Sprite),
    };
}
