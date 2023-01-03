using UnityEngine;

public static class GlobalConfig
{

    public static string AssetBundleDir { get; } = Application.dataPath + "/../Bundle";
    public static string LuaBundleDir { get; } = Application.dataPath + "/../LuaBundle";
    public static string AssetDir { get; } = Application.dataPath + "/Res";
    public static string GameCacheDir { get; } = Application.dataPath + "/../GameCache";
    public static string EditorLuaScriptDir { get; } = Application.dataPath + "/Script/Lua/";
    public static string AssetBundleServerUrlGitee { get; } = "https://gitee.com/purezhao/TripleTown/raw/main/Bundle/";
    public static string LuaBundleServerUrlGitee { get; } = "https://gitee.com/purezhao/TripleTown/raw/main/LuaBundle/";
    //public static string AssetBundleServerUrlGithub { get; } = "https://raw.githubusercontent.com/PureZhao/TripleTown/blob/main/Bundle/";
    public static string AssetBundleListUrl { get; } = "https://gitee.com/purezhao/TripleTown/raw/main/AssetBundleList.json";
    public static string LuaBundleListUrl { get; } = "https://gitee.com/purezhao/TripleTown/raw/main/LuaBundleList.json";

    public static string AssetVersionControlFile { get; } = Application.dataPath + "/../AssetsVersionControl.txt";
    public static string LuaVersionControlFile { get; } = Application.dataPath + "/../LuaVersionControl.txt";
}
