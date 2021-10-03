using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(OverallGameManager gameManager)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saveData";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(gameManager);

        binaryFormatter.Serialize(stream, data);
        stream.Close();

    }

    public static PlayerData LoadData()
    {
        string path = Application.persistentDataPath + "/saveData";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = (PlayerData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }


    public static void DeleteSaveFiles()
    {
        string path = Application.persistentDataPath + "/saveData";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

}
