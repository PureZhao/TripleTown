using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

public static class XLuaCSharpCallLuaType
{
    [CSharpCallLua]
    public static List<Type> CSCallLua = new List<Type>()
    {
        typeof(Func<bool>),
    };
}
