using GameCore;
using LitJson;
using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckUpdate : MonoBehaviour
{
    public Text tip;
    public Text counter;
    IEnumerator Start()
    {
        if (!Directory.Exists(GlobalConfig.AssetBundleDir))
        {
            Directory.CreateDirectory(GlobalConfig.AssetBundleDir);
        }
        yield return StartCoroutine(DownloadAssetsBundleList());
        yield return StartCoroutine(DownloadLuaBundleList());
        SceneManager.LoadScene("Game");
    }

    IEnumerator DownloadAssetsBundleList()
    {
        Debug.Log("Check Assets Version");
        tip.text = "Check Assets Version";
        counter.text = "";
        float progress = 0f;
        UnityWebRequest request = UnityWebRequest.Get(GlobalConfig.AssetBundleListUrl);
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
                Debug.Log(content);
                JsonData data = JsonMapper.ToObject(content);
                // �԰汾
                string version = data[0].ToString();
                if (IsVersionEqual(version, GlobalConfig.AssetVersionControlFile))
                {
                    Debug.Log("Version is latest, need not update");
                    tip.text = "Version is latest, need not update";
                    yield return new WaitForSeconds(1f);
                    yield break;
                }
                else
                {
                    Debug.Log("Version is not latest, run update");
                    tip.text = "Version is not latest, run update";
                    counter.text = "0/" + data.Count.ToString();
                    // ����ǰ����Ҫ��MD5�룬��������ѹ��
                    yield return StartCoroutine(DownloadAssets(data));
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
                    counter.text = (i+1).ToString() + "/" + data.Count.ToString();
                    progress = 1;
                    byte[] bytes = request.downloadHandler.data;
                    SaveAsset(localDir, filename, bytes);
                }
            }
        }
        // ���°汾�����ļ�
        if (File.Exists(GlobalConfig.AssetVersionControlFile))
        {
            File.Delete(GlobalConfig.AssetVersionControlFile);
        }
        FileStream stream = File.Create(GlobalConfig.AssetVersionControlFile);
        byte[] versionContent = data[0].ToString().ToByteArray();
        stream.Write(versionContent, 0, versionContent.Length);
        Debug.Log("Update Finished");
        tip.text = "Update Finished";
        yield return new WaitForSeconds(2f);
    }

    IEnumerator DownloadLuaBundleList()
    {
        Debug.Log("Check Scripts Version");
        tip.text = "Check Scripts Version";
        counter.text = "";
        float progress = 0f;
        UnityWebRequest request = UnityWebRequest.Get(GlobalConfig.LuaBundleListUrl);
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
                if (IsVersionEqual(version, GlobalConfig.LuaVersionControlFile))
                {
                    Debug.Log("Version is latest, need not update");
                    tip.text = "Version is latest, need not update";
                    yield return new WaitForSeconds(1f);
                    yield break;
                }
                else
                {
                    Debug.Log("Version is not latest, run update");
                    tip.text = "Version is not latest, run update";
                    counter.text = "0/" + data.Count.ToString();
                    // ����ǰ����Ҫ��MD5�룬��������ѹ��
                    yield return StartCoroutine(DownloadLua(data));
                }

            }
        }
    }

    IEnumerator DownloadLua(JsonData data)
    {
        // ��ȡ����lua·��
        for (int i = 1; i < data.Count; i++)
        {
            // ��Gitee��raw.githubusercontent.com��ʱ����������
            string httpUrl = Path.Combine(GlobalConfig.LuaBundleServerUrlGitee, data[i].ToString());
            string filename = Path.GetFileName(httpUrl);
            string localUrl = Path.Combine(GlobalConfig.LuaBundleDir, data[i].ToString());
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
                    counter.text = (i + 1).ToString() + "/" + data.Count.ToString();
                    progress = 1;
                    byte[] bytes = request.downloadHandler.data;
                    SaveAsset(localDir, filename, bytes);
                }
            }
        }
        // ���°汾�����ļ�
        if (File.Exists(GlobalConfig.LuaVersionControlFile))
        {
            File.Delete(GlobalConfig.LuaVersionControlFile);
        }
        FileStream stream = File.Create(GlobalConfig.LuaVersionControlFile);
        byte[] versionContent = data[0].ToString().ToByteArray();
        stream.Write(versionContent, 0, versionContent.Length);
        Debug.Log("Update Finished");
        tip.text = "Update Finished";
        yield return new WaitForSeconds(2f);
    }

    private bool IsVersionEqual(string netVersion, string versionControlFile)
    {
        if (!File.Exists(versionControlFile))
            return false;
        return File.ReadAllText(versionControlFile) == netVersion;
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
