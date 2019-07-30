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
    public bool stageClear;
}

public class JsonManager : MonoBehaviour
{    
    public void Save(int stage_index, bool clear)
    {
        SaveData[] preloadedSaveData = Load<SaveData>("SaveData", "Save.json");

        preloadedSaveData[stage_index - 1].stageClear = clear;

        SaveData[] saveData = preloadedSaveData;

        string toJson = JsonHelper.ToJson(saveData, prettyPrint:true);
        string originalPath = Path.Combine(Application.streamingAssetsPath, "SaveData", "Save.json");

        File.WriteAllText(originalPath, toJson);
    }

    public T[] Load<T>(string folder, string fileName)
    {
        // Android
        //string originalPath = Path.Combine(Application.streamingAssetsPath, "JsonData", "Dialogue.json");
        string originalPath = Path.Combine(Application.streamingAssetsPath, folder, fileName);
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
