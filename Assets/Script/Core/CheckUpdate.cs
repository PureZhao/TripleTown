using GameCore;
using LitJson;
using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CheckUpdate : MonoBehaviour
{
    IEnumerator Start()
    {
        if (!Directory.Exists(GlobalConfig.AssetBundleDir))
        {
            Directory.CreateDirectory(GlobalConfig.AssetBundleDir);
        }
        //yield return StartCoroutine(DownloadBundleList());
        yield return new WaitUntil(() =>
        {
            return AssetsManager.Instance != null
            && GameObjectPool.Instane != null
            && Scheduler.Instance != null;
        });
        SceneManager.LoadScene("Game");
    }

    IEnumerator DownloadBundleList()
    {
        Debug.Log("Check Version");
        float progress = 0f;
        UnityWebRequest request = UnityWebRequest.Get(GlobalConfig.BundleListUrl);
        request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {

            Debug.LogError(request.error);
        }
        else
        {
            while (!request.isDone)
            {
                progress = request.downloadProgress * 100f;
                yield return 0;
            }
            if (request.isDone)
            {
                progress = 1;
                byte[] bytes = request.downloadHandler.data;
                string content = Encoding.UTF8.GetString(bytes);
                JsonData data = JsonMapper.ToObject(content);
                // 对版本
                string version = data[0].ToString();
                if (IsVersionEqual(version))
                {
                    Debug.Log("Version is latest, need not update");
                    yield break;
                }
                else
                {
                    Debug.Log("Version is not latest, run update");
                    // 调用前还需要对MD5码，减轻下载压力
                    StartCoroutine(DownloadAssets(data));
                }
                
            }
        }
    }

    // 需要更新才调用
    IEnumerator DownloadAssets(JsonData data)
    {
        // 获取所有bundle
        for(int i = 1;i < data.Count; i++)
        {
            // 用Gitee，raw.githubusercontent.com有时候抽风连不上
            string httpUrl = Path.Combine(GlobalConfig.AssetBundleServerUrlGitee, data[i].ToString());
            string filename = Path.GetFileName(httpUrl);
            string localUrl = Path.Combine(GlobalConfig.AssetBundleDir, data[i].ToString());
            string localDir = Path.GetDirectoryName(localUrl);
            // 后面做一个进度条界面
            float progress = 0f;
            UnityWebRequest request = UnityWebRequest.Get(httpUrl);
            //Debug.Log("下载 " + httpUrl);
            request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {

                Debug.LogError(request.error);
            }
            else
            {
                while (!request.isDone)
                {
                    progress = request.downloadProgress * 100f;
                    yield return 0;
                }
                if (request.isDone)
                {
                    //Debug.Log("下载完成 " + httpUrl);
                    progress = 1;
                    byte[] bytes = request.downloadHandler.data;
                    SaveAsset(localDir, filename, bytes);
                }
            }
        }
        // 更新版本控制文件
        if (File.Exists(GlobalConfig.VersionControlFile))
        {
            File.Delete(GlobalConfig.VersionControlFile);
        }
        FileStream stream = File.Create(GlobalConfig.VersionControlFile);
        byte[] versionContent = data[0].ToString().ToByteArray();
        stream.Write(versionContent, 0, versionContent.Length);
        Debug.Log("Update Finished");
        yield return new WaitForSeconds(2f);
    }

    private bool IsVersionEqual(string netVersion)
    {
        if (!File.Exists(GlobalConfig.VersionControlFile))
            return false;
        return File.ReadAllText(GlobalConfig.VersionControlFile) == netVersion;
    }

    /// <summary>
    /// 保存资源到本地
    /// </summary>
    /// <param name="path">本地保存路径</param>
    /// <param name="filename">文件名称带后缀</param>
    /// <param name="bytes">byte数据</param>
    public void SaveAsset(string path, string filename, byte[] bytes)
    {
        string filePath = Path.Combine(path, filename);
        Stream sw = null;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FileInfo fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists)
        {
            //如果文件存在删除该文件
            File.Delete(filePath);
            //Debug.Log("删除文件:" + filePath);
        }
        sw = fileInfo.Create();
        sw.Write(bytes, 0, bytes.Length);
        sw.Flush();
        sw.Close();
        sw.Dispose();
        //Debug.Log(filename + "成功保存到本地~");
    }

}
