using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;


[Serializable]
public class SaveData
{

    public int currLevel = 0;

    public float MasterAudio = 0f;
    public float EffectAudio = 0f;
    public float MuiscAudio = -20f;

}





public class FileHandler
{

    public enum SaveType { PlayerPrefs, Binary, Json }

    static public SaveType type = SaveType.Json;

    static string binaryPath = Application.persistentDataPath + "/Hulk.dat";
    static string jsonPath = Application.persistentDataPath + "/Hulk.json";

    //static string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    static bool savePresent = false;
    public static bool SavePresent => savePresent;


    public static void ClearSaveData()
    {

        if (PlayerPrefs.HasKey(jsonPath))
        {
            PlayerPrefs.DeleteKey(jsonPath);
        }
        
        
        

        if(File.Exists(jsonPath))
        {
            Debug.Log("JsonDel");
            File.Delete(jsonPath);
        }
        
    }

    

    public static void Save(SaveData data, SaveType savetype)
    {
        
        
        switch (savetype)
        {
            case SaveType.PlayerPrefs:
                {
                    Debug.Log("Playersave");
                    PlayerPrefs.SetInt("currLevel", data.currLevel);
                    PlayerPrefs.Save();
                    break;
                }
                
            case SaveType.Binary:
                {
                    Debug.Log("Binarysave");
                    BinaryFormatter bf = new BinaryFormatter();

                    FileStream fs = File.Create(binaryPath);

                    bf.Serialize(fs, data);

                    fs.Close();

                    break;
                }
            case SaveType.Json:
                {
                    Debug.Log("Jsonsave");
                    string json = JsonUtility.ToJson(data);

                    File.WriteAllText(jsonPath, json);




                    break;
                }
                
        }
    } 

    public static SaveData Load(SaveType savetype)
    {

        SaveData data = new SaveData();

        

        switch (savetype)
        {
            case SaveType.PlayerPrefs:
                {
                    Debug.Log("Playerload");
                    if (PlayerPrefs.HasKey("currLevel"))
                    {
                        data.currLevel = PlayerPrefs.GetInt("currLevel");
                        savePresent = true;
                    }
                    break;
                }

            case SaveType.Binary:
                {
                    Debug.Log("Binaryload");
                    if (File.Exists(binaryPath))
                    {
                        savePresent = true;

                        BinaryFormatter bf = new BinaryFormatter();

                        FileStream fs = File.Open(binaryPath, FileMode.Open);

                        data = (SaveData)bf.Deserialize(fs);

                        fs.Close();
                    }

                    break;
                }
            case SaveType.Json:
                {
                    Debug.Log("Jsonload");
                    if (File.Exists(jsonPath))
                    {
                        savePresent = true;

                        string json = File.ReadAllText(jsonPath);
                        data = JsonUtility.FromJson<SaveData>(json);   
                    }
                    break;
                }

        }

        return data;
    }

}
