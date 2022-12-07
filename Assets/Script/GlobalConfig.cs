using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public static class GlobalConfig
{

    public static string AssetBundleDir { get; } = Application.dataPath + "/../Bundle";
    public static string AssetDir { get; } = Application.dataPath + "/Res";
    public static string GameCacheDir { get; } = Application.dataPath + "/../GameCache";
    public static string LuaScriptDir { get; } = Application.dataPath + "/Script/Lua/";
    public static string AssetBundleServerUrlGitee { get; } = "https://gitee.com/purezhao/TripleTown/raw/main/Bundle/";
    public static string AssetBundleServerUrlGithub { get; } = "https://raw.githubusercontent.com/PureZhao/TripleTown/blob/main/Bundle/";
    public static string BundleListUrl { get; } = "https://gitee.com/purezhao/TripleTown/raw/main/BundleList.json";
    public static string BundleListLocalUrl { get; } = Application.dataPath + "/../BundleList.json";

    public static string VersionControlFile { get; } = AssetBundleDir + "VersionControl.txt";
}
