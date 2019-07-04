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
    public static JsonManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    
    public void Save()
    {

    }

    public T[] Load<T>()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/JsonData/Dialogue.json");
        T[] data = JsonHelper.FromJson<T>(jsonString);

        return data;
    }
}
