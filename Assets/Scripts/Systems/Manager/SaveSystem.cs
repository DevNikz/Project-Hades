using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayerData(SaveManager player, int index) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + $"/playerSave{index}.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerStats data = new PlayerStats(player); 

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static PlayerStats LoadPlayerData(int index) {
        string path = Application.persistentDataPath + $"/playerSave{index}.sav";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerStats data = formatter.Deserialize(stream) as PlayerStats;
            stream.Close();

            return data;
        }
        else {
            return null;
        }
    }
}