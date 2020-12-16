using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour, IGameModule
{
    private string saveFolderPath;

    public void Init()
    {
        saveFolderPath = Application.dataPath + "/Saves/";

        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }

    //Saving and Loading JSon
    public void SaveJson<T>(T saveObject, string fileName)
    {
        if (!fileName.Contains(".txt"))
        {
            Debug.LogError("Error: File should contains extension .txt");
            return;
        }

        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(saveFolderPath + "/" + fileName, json);
    }

    public T LoadJson<T>(string fileName)
    {
        if (!fileName.Contains(".txt"))
        {
            Debug.LogError("Error: File should contains extension .txt");
            return default(T);
        }

        string savedString = File.ReadAllText(saveFolderPath + "/" + fileName);

        return JsonUtility.FromJson<T>(savedString);
    }
    //
    
    //Unity PlayerPrefs
    public void SavePlayerPrefs(float data, string key)
    {
        PlayerPrefs.SetFloat(key, data);
        PlayerPrefs.Save();
    }

    public void SavePlayerPrefs(int data, string key)
    {
        PlayerPrefs.SetInt(key, data);
        PlayerPrefs.Save();
    }

    public void SavePlayerPrefs(string data, string key)
    {
        PlayerPrefs.SetString(key, data);
        PlayerPrefs.Save();
    }

    public float LoadFloat(string key)
    {
        return PlayerPrefs.GetFloat(key, 0.0f);
    }

    public int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }

    public string LoadString(string key)
    {
        return PlayerPrefs.GetString(key, "");
    }

    public void ClearKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    public void ClearSavedPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    //

    //Saving Binaries
    public void SaveBinary<T>(T data, string fileName)
    {
        FileStream dataStream = new FileStream(saveFolderPath + "/" + fileName + ".data", FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, data);

        dataStream.Close();
    }

    public T LoadBinary<T>(string fileName)
    {
        string filePath = saveFolderPath + "/" + fileName + ".data";

        if (File.Exists(filePath))
        {
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            T savedData = (T)converter.Deserialize(dataStream);

            dataStream.Close();

            return savedData;
        }
        else
        {
            Debug.LogError("Save file not found in " + filePath);
            return default(T);
        }
    }
    //

    public IEnumerator LoadModule()
    {
        Init();
        ServiceLocator.Register<SaveSystem>(this);
        yield return null;
    }

     
}
