using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using DG.Tweening;
public static class XLuaLuaCallCSharpType
{
    [LuaCallCSharp]
    public static List<Type> LuaCallCharp = new List<Type>()
    {
        typeof(WaitForSeconds),
        typeof(WaitUntil),
        typeof(Rect),
        typeof(Sprite),
        typeof(ShortcutExtensions),
        typeof(TweenSettingsExtensions),
        typeof(DOTweenModuleUI),
        typeof(TweenExtensions),
    };
}
