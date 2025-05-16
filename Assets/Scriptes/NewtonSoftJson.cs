using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class NewtonSoftJson
{
    public static string ObjectToJson(object _obj) 
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Converters.Add(new Vector2Converter());
        settings.Converters.Add(new Vector3Converter());

        return JsonConvert.SerializeObject(_obj, settings);
    }
    public static T JsonToOject<T>(string _jsonData) 
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Converters.Add(new Vector2Converter());
        settings.Converters.Add(new Vector3Converter());

        return JsonConvert.DeserializeObject<T>(_jsonData, settings); 
    }
    public static void CreateJsonFile(string _createPath, string _fileName, string _jsonData) 
    {
        string filePath = string.Format("{0}/{1}.json", _createPath, _fileName);

        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                byte[] data = Encoding.UTF8.GetBytes(_jsonData);
                fileStream.Write(data, 0, data.Length);
                fileStream.Close();
            }

            Debug.Log($"JSON file created successfully at: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create JSON file. Exception: {e.Message}");
        }
    }
    public static T LoadJsonFile<T>(string _loadPath, string _fileName) 
    {
        string filePath = string.Format("{0}/{1}.json", _loadPath, _fileName);

        if (File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(Path.Combine(Application.dataPath + _loadPath + _fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonToOject<T>(jsonData);
        }
        else
        {
            Debug.LogWarningFormat("Utile LoadJson Warning \n filePath : {0}, _loadPath : {1}, _fileName : {2}}", filePath, _loadPath, _fileName);
            return default;
        }
    }
    public static List<T> LoadJsonArray<T>(string _loadPath, string _fileName)
    {
        string filePath = string.Format("{0}/{1}.json", _loadPath, _fileName);

        if (File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonToOject<List<T>>(jsonData);
        }
        else
        {
            Debug.LogWarningFormat("Util LoadJsonArray Warning \n {0}", filePath);
            return new List<T>(); // 또는 예외 처리를 추가하여 반환값을 선택할 수 있습니다.
        }
    }
}
