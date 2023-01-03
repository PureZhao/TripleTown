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
            luaScriptPath = GlobalConfig.EditorLuaScriptDir;
        }

        [Button("Export LuaBundle", ButtonSizes.Large)]
        public void ExportLuaBundle()
        {
            string originDir = GlobalConfig.LuaBundleDir + "Origin";
            List<string> paths = new List<string>();
            GetAllFile(GlobalConfig.EditorLuaScriptDir, ref paths);
            if (Directory.Exists(originDir))
            {
                Directory.Delete(originDir, true);
            }
            Directory.CreateDirectory(originDir);

            foreach (string path in paths)
            {

                string relativePath = path.Remove(0, GlobalConfig.EditorLuaScriptDir.Length);
                //Debug.Log(relativePath);
                string filepath = Path.Combine(originDir, relativePath + ".bytes");
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
            GetAllFile(GlobalConfig.EditorLuaScriptDir, ref path);
            data.Add("v" + version);
            foreach (string p in path)
            {
                //Debug.Log(p);
                string realPath = p.Remove(0, GlobalConfig.EditorLuaScriptDir.Length).Replace('\\', '/') + ".bytes";
                data.Add(realPath);
            }
            string jsonStorePath = Application.dataPath + "/../LuaBundleList.json";
            JsonHelper.WriteJson2File(data, jsonStorePath);
        }
        [Button("Conver To ByteCode", ButtonSizes.Large)]
        public void Conver2ByteCode()
        {
            List<string> paths = new List<string>();
            string originDir = GlobalConfig.LuaBundleDir + "Origin";
            GetAllFile(originDir, ref paths);
            if (Directory.Exists(GlobalConfig.LuaBundleDir))
            {
                Directory.Delete(GlobalConfig.LuaBundleDir, true);
            }
            Directory.CreateDirectory(GlobalConfig.LuaBundleDir);
            DirectoryInfo directoryInfo = new DirectoryInfo(originDir);
            string cmds = "";
            foreach (string path in paths)
            {
                string relativePath = path.Remove(0, directoryInfo.FullName.Length + 1);
                string filepath = GlobalConfig.LuaBundleDir + "/" + relativePath;
                string dir = Path.GetDirectoryName(filepath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string input = "\"" + path + "\"";
                string output = "\"" + filepath.Remove("Assets/../") + "\"";
                cmds += string.Format("luac53 -o {0} {1}\n", output, input).Replace('\\', '/');
                //Debug.Log(cmd);
                //Encrypt(cmd);
            }
            string cmdFile = Application.dataPath + "/../cmd.txt";
            byte[] cmdsBytes = cmds.ToByteArray();
            FileStream stream = File.Create(cmdFile);
            stream.Write(cmdsBytes, 0, cmdsBytes.Length);
            stream.Close();
            stream.Dispose();
        }

        public void Encrypt(string cmd)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            System.Diagnostics.Process p = new System.Diagnostics.Process()
            {
                StartInfo = info,
            };
            p.Start();
            p.StandardInput.WriteLine(cmd + " &exit");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();
            p.Close();
            
        }
    }
}