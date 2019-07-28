using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[System.Serializable]
public class Dialogue
{
    public int index;
    public string name;
    public string content;
    public string target;
}

[System.Serializable]
public class SaveData
{
    public int count;
    public float HP;
}

public class JsonManager : MonoBehaviour
{
    
    
    public void Save()
    {

    }

    public T[] Load<T>()
    {
        // Android
        string originalPath = Path.Combine(Application.streamingAssetsPath, "JsonData", "Dialogue.json");

        // Android only use WWW to read file
        WWW reader = new WWW(originalPath);
        while(!reader.isDone){ }

        string realPath = Application.persistentDataPath + "/dataBase";
        File.WriteAllBytes(realPath, reader.bytes);

        string jsonString = File.ReadAllText(realPath);

        T[] data = JsonHelper.FromJson<T>(jsonString);

        return data;
    }     
}
