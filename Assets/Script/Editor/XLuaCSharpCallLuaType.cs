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
        typeof(Action),
        typeof(Action<string>),
        typeof(Action<double>),
        typeof(UnityEngine.Events.UnityAction),
        typeof(System.Collections.IEnumerator),
        typeof(Func<bool>),
        typeof(Func<double, double, double>),
    };
}
