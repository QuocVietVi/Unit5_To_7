using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveData<T>(List<T> data)
    {
        string content = JsonHelper.ToJson(data.ToArray());
        string path = Application.persistentDataPath + "/" + typeof(T).ToString() + ".json";
        WriteFile(path, content);
    }
    public void SaveData<T>(T data)
    {
        string content = JsonConvert.SerializeObject(data);
        string path = Application.persistentDataPath + "/" + typeof(T).ToString() + ".json";
        WriteFile(path, content);
    }


    public List<T> ReadListData<T>()
    {
        Debug.Log("read dataa");
        string path = Application.persistentDataPath + "/" + typeof(T).ToString() + ".json";
        string content = ReadFile(path);
        if (string.IsNullOrEmpty(content))
        {
            return new List<T>();
        }
        List<T> result = JsonHelper.FromJson<T>(content).ToList();
        return result;
    }
    public T ReadData<T>()
    {
        Debug.Log("read data");
        string path = Application.persistentDataPath + "/" + typeof(T).ToString() + ".json";
        string content = ReadFile(path);
        if (string.IsNullOrEmpty(content))
        {
            return default(T);
        }
        T result = JsonConvert.DeserializeObject<T>(content);
        return result;
    }

    public bool HasData<T>()
    {
        return File.Exists(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
    }

    private void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    private string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }

        }
        return "";
    }


}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Animals;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Animals = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Animals = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Animals;
    }
}
