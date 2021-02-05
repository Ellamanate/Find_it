using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

public static class SaveLoad<T>
{
    private static string path = "find_it";

    static public void Save(T data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + path + ".gd");
        bf.Serialize(file, JsonUtility.ToJson(data));
        file.Close();
        Debug.Log("Save");
    }

    static public T Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + path + ".gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + path + ".gd", FileMode.Open);
            string json = (string)bf.Deserialize(file);
            file.Close();
            T saves = JsonUtility.FromJson<T>(json);
            return saves;
        }
        else
            Debug.Log("No Saves");

        return default;
    }
}

[Serializable]
public class GameState
{
    public int CurrentLevelIndex;
    public List<ItemState> ItemStates = new List<ItemState>();

    public GameState(int currentLevelIndex, List<ItemState> itemStates)
    {
        CurrentLevelIndex = currentLevelIndex;
        ItemStates = itemStates;
    }
}
