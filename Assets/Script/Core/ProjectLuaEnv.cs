using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;
public class ProjectLuaEnv
{
    private static ProjectLuaEnv instance;

    public static ProjectLuaEnv Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ProjectLuaEnv();
            }
            return instance;
        }
    }

    private readonly LuaEnv luaEnv;

    private ProjectLuaEnv()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(ProjectLoader);
    }

    private byte[] ProjectLoader(ref string path)
    {
        string relativePath = path.Replace('.', '/') + ".lua.bytes";
        string fullPath = Path.Combine(GlobalConfig.LuaBundleDir, relativePath);
#if UNITY_EDITOR
        relativePath = path.Replace('.', '/') + ".lua";
        fullPath = Path.Combine(GlobalConfig.EditorLuaScriptDir, relativePath);
#endif
        //Debug.Log(fullPath);
        if (File.Exists(fullPath))
        {
            return File.ReadAllBytes(fullPath);
        }
        else
        {
            return null;
        }
    }

    public LuaTable Global
    {
        get
        {
            return luaEnv.Global;
        }
    }

    public LuaTable NewTable()
    {
        return luaEnv.NewTable();
    }

    public object[] DoString(string cmd)
    {
        return luaEnv.DoString(cmd);
    }

    public void Dispose()
    {
        luaEnv.Dispose();
        instance = null;
    }

    public LuaTable Get(object name)
    {
        return luaEnv.Global.Get<object, LuaTable>(name);
    }

    public void Tick()
    {
        luaEnv.Tick();
    }

}