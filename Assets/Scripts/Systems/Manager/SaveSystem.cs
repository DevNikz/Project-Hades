using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(SaveManager player) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerSave.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerStats data = new PlayerStats(player); 

        formatter.Serialize(stream, data);

        Debug.Log("Saved Player!");
        stream.Close();
    }

    public static PlayerStats LoadPlayer() {
        string path = Application.persistentDataPath + "/player.kek";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerStats data = formatter.Deserialize(stream) as PlayerStats;
            stream.Close();

            Debug.Log("Loaded Player File!");
            return data;
        }
        else {
            return null;
        }
    }
}