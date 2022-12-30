using LitJson;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Util;

namespace PureOdinTools
{
    [GlobalConfig(assetPath: "Odin")]
    public class LuaBundleTool : GlobalConfig<LuaBundleTool>
    {
        [ReadOnly]
        public string luaScriptPath;
        private void OnEnable()
        {
            luaScriptPath = GlobalConfig.LuaScriptDir;
        }

        [Button("Export LuaBundle", ButtonSizes.Large)]
        public void ExportLuaBundle()
        {

            List<string> paths = new List<string>();
            GetAllFile(GlobalConfig.LuaScriptDir, ref paths);
            if (Directory.Exists(GlobalConfig.LuaBundleDir))
            {
                Directory.Delete(GlobalConfig.LuaBundleDir, true);
            }
            Directory.CreateDirectory(GlobalConfig.LuaBundleDir);
            foreach(string path in paths)
            {
                string relativePath = path.Remove("Assets\\Script\\Lua\\");
                string filepath = Path.Combine(GlobalConfig.LuaBundleDir, relativePath + ".bytes");
                string dir = Path.GetDirectoryName(filepath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                //Debug.Log(dir);
                byte[] bytes = File.ReadAllBytes(path);
                FileStream stream = File.Create(filepath);
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                stream.Dispose();
            }
        }


        private void GetAllFile(string dir, ref List<string> paths)
        {
            DirectoryInfo root = new DirectoryInfo(dir);
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".meta" || file.FullName.Contains(".vscode") || file.Name.Contains("LuaPanda"))
                {
                    continue;
                }
                paths.Add(file.FullName);
            }
            DirectoryInfo[] subDirs = root.GetDirectories();
            foreach (DirectoryInfo directory in subDirs)
            {
                GetAllFile(Path.Combine(dir, directory.Name), ref paths);
            }
        }

        [Button("Export Version Control File")]
        public void ExportVersionControlBundle(string version)
        {
            JsonData data = new JsonData();
            data.SetJsonType(JsonType.Array);
            List<string> path = new List<string>();
            GetAllFile(GlobalConfig.LuaBundleDir, ref path);
            string key = "LuaBundle";
            int keyPos = -1;
            data.Add("v" + version);
            foreach (string p in path)
            {
                if(keyPos == -1)
                {
                    keyPos = p.IndexOf(key);
                }
                //Debug.Log(p);
                string realPath = p.Substring(keyPos);
                data.Add(realPath);
            }
            string jsonStorePath = Application.dataPath + "/../LuaBundleList.json";
            JsonHelper.WriteJson2File(data, jsonStorePath);
        }
    }
}