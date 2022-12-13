using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;
using UnityEngine.SceneManagement;
using XLua;
using UnityObject = UnityEngine.Object;

namespace GameCore
{
    [LuaCallCSharp]
    public class AssetsManager : MonoBehaviour
    {
        private static AssetsManager instance;
        public static AssetsManager Instance { get => instance; }
        private Dictionary<string, string> path2bundle = new Dictionary<string, string>();
        private Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();
        private AssetBundleManifest mainManifest;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
            DontDestroyOnLoad(this);
            // ��ʼ����Bundle �������������
            // ��AB��һ�����ڣ������뵼��ʱ���ļ�������ͬ
            DirectoryInfo directory = new DirectoryInfo(GlobalConfig.AssetBundleDir);
            string mainPath = Path.Combine(GlobalConfig.AssetBundleDir, directory.Name);
            AssetBundle main = AssetBundle.LoadFromFile(mainPath);
            // ��ȡ��AB���������ļ�
            mainManifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            // ����Դ·����bundleӳ���ȼ��س���
            string mappingFile = Path.Combine(GlobalConfig.AssetBundleDir, "main");
            AssetBundle bundle = AssetBundle.LoadFromFile(mappingFile);
            bundles.Add("main", bundle);
            TextAsset mappingText = bundle.LoadAsset<TextAsset>("AssetMapping.json");
            JsonReader reader = new JsonReader(mappingText.ToString());
            JsonData data = JsonMapper.ToObject(reader);
            foreach (string key in data.Keys)
            {
                path2bundle.Add(key, data[key].ToString());
            }
            reader.Close();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            AssetBundle.UnloadAllAssetBundles(true);
            path2bundle.Clear();
            bundles.Clear();
        }

        private void LoadAssetBundle(string bundleName, Action<AssetBundle> onLoaded)
        {
            // ���������ڵ�����
            List<string> deps = CheckNoneExistedDependencies(bundleName);
            // ���û�в����ڵ�ֱ�ӷ���
            if (deps.Count == 0)
            {
                onLoaded?.Invoke(bundles[bundleName]);
            }
            else
            {
                // �첽����
                StartCoroutine(LoadAssetBundlesAsync(deps, bundleName, onLoaded));
            }
        }

        IEnumerator LoadAssetBundlesAsync(List<string> deps, string targetBundleName, Action<AssetBundle> onLoaded)
        {
            for (int i = 0; i < deps.Count; i++)
            {
                if (bundles.ContainsKey(deps[i]))
                {
                    // ��ֹ�ظ�����
                    yield return new WaitUntil(() => bundles[deps[i]] != null);
                }
                else
                {
                    string bundlePath = Path.Combine(GlobalConfig.AssetBundleDir, deps[i]);
                    // ռλ��
                    bundles.Add(deps[i], null);
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
                    yield return request;
                    bundles[deps[i]] = request.assetBundle;
                }   
            }
            onLoaded?.Invoke(bundles[targetBundleName]);
        }
        public void LoadGameObject(string assetPath, Action<GameObject> onLoaded = null)
        {
            GameObject g = GameObjectPool.Instane.PushOutPull(assetPath);
            if(g != null)
            {
                g.transform.parent = null;
                g.SetActive(true);
                onLoaded?.Invoke(g);
            }
            else
            {
                LoadAsset<UnityObject>(assetPath, (obj) =>
                {
                    g = GameObject.Instantiate(obj) as GameObject;
                    g.transform.position = Vector3.zero;
                    g.transform.rotation = Quaternion.identity;
                    g.SetActive(true);
                    g.GetOrAddComponent<AssetFlag>((comp) =>
                    {
                        comp.Path = assetPath;
                    });
                    onLoaded?.Invoke(g);
                });
            }
        }

        public void LoadAsset(string assetPath, Type type, Action<UnityObject> onLoaded = null)
        {
            if (string.IsNullOrEmpty(assetPath) || string.IsNullOrWhiteSpace(assetPath))
            {
                throw new ArgumentException("Bad Argument: It was NULL , Empty or WhiteSpace");
            }
            if (path2bundle.TryGetValue(assetPath, out string bundleName))
            {
                LoadAssetBundle(bundleName, (bundle) =>
                {
                    FileInfo file = new FileInfo(assetPath);
                    StartCoroutine(LoadAssetAsync(file.Name, bundle, type, onLoaded));
                });

            }
            else
            {
                throw new DirectoryNotFoundException("Bad Key: " + assetPath + " is not a key of bundle dictionary");
            }
        }

        public void LoadAsset<T>(string assetPath, Action<T> onLoaded = null) where T : UnityObject
        {
            if (string.IsNullOrEmpty(assetPath) || string.IsNullOrWhiteSpace(assetPath))
            {
                throw new ArgumentException("Bad Argument: It was NULL , Empty or WhiteSpace");
            }
            if (path2bundle.TryGetValue(assetPath, out string bundleName))
            {
                LoadAssetBundle(bundleName, (bundle) =>
                {
                    FileInfo file = new FileInfo(assetPath);
                    StartCoroutine(LoadAssetAsync<T>(file.Name, bundle, onLoaded));
                });

            }
            else
            {
                throw new DirectoryNotFoundException("Bad Key: " + assetPath + " is not a key of bundle dictionary");
            }
        }

        // ����֮ǰ��֤Bundle����
        IEnumerator LoadAssetAsync(string assetName, AssetBundle bundle, Type t, Action<UnityObject> onLoaded)
        {
            AssetBundleRequest request = bundle.LoadAssetAsync(assetName, t);
            yield return request;
            onLoaded?.Invoke(request.asset);
        }

        IEnumerator LoadAssetAsync<T>(string assetName, AssetBundle bundle, Action<T> onLoaded) where T : UnityObject
        {
            AssetBundleRequest request = bundle.LoadAssetAsync<UnityObject>(assetName);
            yield return request;
            onLoaded?.Invoke(request.asset as T);
        }

        private List<string> CheckNoneExistedDependencies(string bundleName)
        {
            List<string> noneExisted = new List<string>();
            string[] dependencies = mainManifest.GetAllDependencies(bundleName);
            foreach (string dep in dependencies)
            {
                if (!bundles.ContainsKey(dep) || bundles[dep] == null)
                {
                    noneExisted.Add(dep);
                }
            }
            // ҲҪ����Լ��治����
            if (!bundles.ContainsKey(bundleName) || bundles[bundleName] == null)
            {
                noneExisted.Add(bundleName);
            }
            return noneExisted;
        }

        public void LoadScene(string scene)
        {
            StartCoroutine(nameof(LoadSceneAsync), scene);
        }

        private IEnumerator LoadSceneAsync(string scene)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            yield return operation;
        }


        public void FreeObject(UnityObject obj, bool useObjectPool = true)
        {
            if (useObjectPool)
            {
                if (obj is GameObject g)
                {
                    if (g.TryGetComponent(out AssetFlag flag))
                    {
                        g.SetActive(false);
                        GameObjectPool.Instane.PushInPool(g);
                    }
                    else
                    {
                        Destroy(g);
                    }
                }
                else
                {
                    Destroy(obj);
                }

            }
            else
            {
                Destroy(obj);
            }
        }
    }
}
