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
                // �԰汾
                string version = data[0].ToString();
                if (IsVersionEqual(version))
                {
                    Debug.Log("Version is latest, need not update");
                    yield break;
                }
                else
                {
                    Debug.Log("Version is not latest, run update");
                    // ����ǰ����Ҫ��MD5�룬��������ѹ��
                    StartCoroutine(DownloadAssets(data));
                }
                
            }
        }
    }

    // ��Ҫ���²ŵ���
    IEnumerator DownloadAssets(JsonData data)
    {
        // ��ȡ����bundle
        for(int i = 1;i < data.Count; i++)
        {
            // ��Gitee��raw.githubusercontent.com��ʱ����������
            string httpUrl = Path.Combine(GlobalConfig.AssetBundleServerUrlGitee, data[i].ToString());
            string filename = Path.GetFileName(httpUrl);
            string localUrl = Path.Combine(GlobalConfig.AssetBundleDir, data[i].ToString());
            string localDir = Path.GetDirectoryName(localUrl);
            // ������һ������������
            float progress = 0f;
            UnityWebRequest request = UnityWebRequest.Get(httpUrl);
            //Debug.Log("���� " + httpUrl);
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
                    //Debug.Log("������� " + httpUrl);
                    progress = 1;
                    byte[] bytes = request.downloadHandler.data;
                    SaveAsset(localDir, filename, bytes);
                }
            }
        }
        // ���°汾�����ļ�
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
    /// ������Դ������
    /// </summary>
    /// <param name="path">���ر���·��</param>
    /// <param name="filename">�ļ����ƴ���׺</param>
    /// <param name="bytes">byte����</param>
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
            //����ļ�����ɾ�����ļ�
            File.Delete(filePath);
            //Debug.Log("ɾ���ļ�:" + filePath);
        }
        sw = fileInfo.Create();
        sw.Write(bytes, 0, bytes.Length);
        sw.Flush();
        sw.Close();
        sw.Dispose();
        //Debug.Log(filename + "�ɹ����浽����~");
    }

}
