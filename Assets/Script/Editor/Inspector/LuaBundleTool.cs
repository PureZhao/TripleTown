using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
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
                string relativePath = "Assets" + file.FullName.Remove(0, Application.dataPath.Length);
                paths.Add(relativePath);
            }
            DirectoryInfo[] subDirs = root.GetDirectories();
            foreach (DirectoryInfo directory in subDirs)
            {
                GetAllFile(Path.Combine(dir, directory.Name), ref paths);
            }
        }

    }
}