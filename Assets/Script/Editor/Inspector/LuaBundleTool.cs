using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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