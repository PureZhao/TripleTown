using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

public static class XLuaBlackList
{
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()
    {
        new List<string>(){"GameCore.LuaBehaviour", "luaScript" },
        new List<string>(){"GameCore.LuaBehaviour", "Rebuild" },
        new List<string>(){"GameCore.AssetsManager", "LoadAsset<T>" },
        new List<string>(){"UnityEngine.Light", "shadowRadius"},
        new List<string>(){"UnityEngine.Light", "SetLightDirty"},
        new List<string>(){"UnityEngine.Light", "shadowAngle"},
        new List<string>(){"UnityEngine.Light", "shadowAngle"}
    };
}
