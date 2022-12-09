using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System;
using System.IO;
using System.Text;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Util;
using System.Linq;

namespace PureOdinTools {
    [GlobalConfig(assetPath: "Odin")]
    public class AutoSetAssetBundleTool : GlobalConfig<AutoSetAssetBundleTool>
    {
        [ReadOnly]
        public string assetRootPath;

        [ReadOnly]
        [ShowInInspector]
        [PropertyOrder(1)]
        [DictionaryDrawerSettings(KeyLabel = "Asset Path", ValueLabel = "Bundle")]
        Dictionary<string, string> assetBundleNames = new Dictionary<string, string>();

        private void OnEnable()
        {
            assetRootPath = GlobalConfig.AssetDir;
        }

        [TitleGroup("ForBundle")]
        [HorizontalGroup("ForBundle/Bundle")]
        [Button("Empty All Bundle", ButtonSizes.Large)]
        public void EmptyAllBundle()
        {
            List<string> allAssetPath = new List<string>();
            GetAllFile(assetRootPath, ref allAssetPath);
            foreach (string path in allAssetPath)
            {
                AssetImporter importer = AssetImporter.GetAtPath(path);
                importer.assetBundleName = string.Empty;
            }
        }

        [TitleGroup("ForBundle")]
        [HorizontalGroup("ForBundle/Bundle")]
        [Button("Set All AssetBundle", ButtonSizes.Large)]
        public void SetAllAssetBundle()
        {
            Dictionary<string, string> assetBundlleMapping = new Dictionary<string, string>();
            List<string> allAssetPath = new List<string>();
            GetAllFile(assetRootPath, ref allAssetPath);

            foreach (string path in allAssetPath)
            {
                FileInfo file = new FileInfo(path);
                // 去掉资源根路径
                // 根据文件夹目录计算Bundle
                string tidedFilePath = file.DirectoryName.Replace('\\', '/');
                string tidedFileRootDir = assetRootPath.Replace('\\', '/');
                // 这个地方替换完成后，如果不是空串，第一个位置是/左斜杠，这是不对的bundlename
                string bundleName = tidedFilePath.Remove(0, tidedFileRootDir.Length);
                if (bundleName == string.Empty)
                {
                    // 在根目录的资源打到一个统一的Bundle下
                    bundleName = "main";
                }
                else if(file.Extension == ".lua")
                {
                    // lua 文件同一打到lua的bundle下
                    bundleName = "lua";
                }
                else
                {
                    bundleName = bundleName.Remove(0, 1);
                }
                AssetImporter importer = AssetImporter.GetAtPath(path);
                importer.assetBundleName = bundleName;
            }

            GenerateBundleMapping();
        }
        [TitleGroup("ForBundle")]
        [HorizontalGroup("ForBundle/Bundle")]
        [Button("Export Asset Bundle", ButtonSizes.Large)]
        public void ExportAssetBundle()
        {
            SetAllAssetBundle();

            string path = GlobalConfig.AssetBundleDir;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
            BuildPipeline.BuildAssetBundles(
                path,
                BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
                BuildTarget.StandaloneWindows);
            Debug.Log("Export Success");
        }

        [TitleGroup("ForBundle")]
        [HorizontalGroup("ForBundle/Bundle")]
        [Button("Export Scenes", ButtonSizes.Large)]
        public void ExportScenes()
        {
            string path = GlobalConfig.AssetBundleDir;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
            BuildPipeline.BuildAssetBundles(
                path,
                BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.ForceRebuildAssetBundle,
                BuildTarget.StandaloneWindows);
            Debug.Log("Export Success");
        }

        void GenerateBundleMapping()
        {
            Dictionary<string, string> assetBundlleMapping = new Dictionary<string, string>();
            List<string> assetPath = new List<string>();
            GetAllFile(assetRootPath, ref assetPath);
            //JsonData json = new JsonData();
            foreach (string path in assetPath)
            {
                AssetImporter importer = AssetImporter.GetAtPath(path);
                string tidedPath = path.Replace('\\', '/');
                assetBundlleMapping.Add(tidedPath, importer.assetBundleName);
            }

            string jsonFile = Path.Combine(assetRootPath, "AssetMapping.json");
            if (File.Exists(jsonFile))
            {
                File.Delete(jsonFile);
            }
            FileStream stream = File.Create(jsonFile);
            string jsonString = JsonMapper.ToJson(assetBundlleMapping);
            byte[] jsonBytes = jsonString.ToByteArray();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Close();
            // 再把这个json文件打成bundle，而且是main的bundle 方便游戏开始直接读取
            AssetImporter jsonImporter = AssetImporter.GetAtPath("Assets/Res/AssetMapping.json");
            jsonImporter.assetBundleName = "main";
        }

        [Button("Browse", ButtonSizes.Large)]
        public void BrowseAllAsset()
        {
            assetBundleNames.Clear();
            List<string> allAssetPath = new List<string>();
            GetAllFile(assetRootPath, ref allAssetPath);

            foreach(string path in allAssetPath)
            {
                FileInfo file = new FileInfo(path);
                //Debug.Log(file.FullName);
                // 去掉资源根路径
                // 根据文件夹目录计算Bundle
                string tidedFilePath = file.DirectoryName.Replace('\\', '/');
                string tidedFileRootDir = assetRootPath.Replace('\\', '/');
                // 这个地方替换完成后，如果不是空串，第一个位置是/左斜杠，这是不对的bundlename
                string bundleName = tidedFilePath.Remove(0, tidedFileRootDir.Length);
                if(bundleName == string.Empty)
                {
                    // 在根目录的资源打到一个统一的Bundle下
                    bundleName = "main";
                }
                else if (file.Extension == ".lua")
                {
                    // lua 文件同一打到lua的bundle下
                    bundleName = "lua";
                }
                else
                {
                    bundleName = bundleName.Remove(0, 1);
                }
                assetBundleNames.Add(path, bundleName);
            }
        }

        private void GetAllFile(string dir, ref List<string> paths)
        {
            DirectoryInfo root = new DirectoryInfo(dir);
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".meta" || file.FullName.Contains(".vscode") || file.Name.Contains("LuaPanda")
                    || file.Extension == ".lua")
                {
                    continue;
                }
                string relativePath = "Assets" + file.FullName.Remove(0, Application.dataPath.Length);
                paths.Add(relativePath);
            }
            DirectoryInfo[] subDirs = root.GetDirectories();
            foreach(DirectoryInfo directory in subDirs)
            {
                GetAllFile(Path.Combine(dir, directory.Name), ref paths);
            }
        }
        [Button("Export Version Control File")]
        public void ExportVersionControlBundle()
        {
            JsonData data = new JsonData();
            data.SetJsonType(JsonType.Array);
            List<string> path = new List<string>();
            GetAllFile(GlobalConfig.AssetBundleDir, ref path);
            data.Add("v0.0.1");
            foreach(string p in path)
            {
                string realPath = p.Remove("Assets\\");
                data.Add(realPath);
            }
            string jsonStorePath = Path.Combine(GlobalConfig.AssetBundleDir, "BundleList.json");
            JsonHelper.WriteJson2File(data, jsonStorePath);
        }

    }
}
