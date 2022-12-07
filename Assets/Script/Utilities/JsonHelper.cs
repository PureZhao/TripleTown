using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Util
{
    public static class JsonHelper
    {
        public static JsonData Vector2Warp(Vector2 v)
        {
            double x = ((int)(v.x * 1000f)) / 1000f;
            double y = ((int)(v.y * 1000f)) / 1000f;
            JsonData jsonData = new JsonData
            {
                ["x"] = x,
                ["y"] = y
            };
            return jsonData;
        }

        public static Vector2 JsonToVector2(JsonData data)
        {
            float x = float.Parse(data["x"].ToString());
            float y = float.Parse(data["y"].ToString());
            return new Vector2(x, y);
        }

        public static void WriteJson2File(JsonData jsonData, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileStream fileStream = File.Create(filePath);
            string jsonString = JsonMapper.ToJson(jsonData);
            fileStream.Write(jsonString.ToByteArray(), 0, jsonString.Length);
            fileStream.Close();
            fileStream.Dispose();
        }

        public static JsonData GetJsonData(TextAsset text)
        {
            JsonReader reader = new JsonReader(text.text);
            return JsonMapper.ToObject(reader); 
        }

    }

    
}