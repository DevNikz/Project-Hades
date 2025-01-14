using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Title("Stats")]

    [ReadOnly, SerializeReference] public int Runs;
    [ReadOnly, SerializeReference] public int DepthLevel;
    [ReadOnly, SerializeReference] public int Wins;
    [ReadOnly, SerializeReference] public int Deaths;
    [ReadOnly, SerializeReference] public bool hasPlayed;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        /* Save
            - persistentDataPath = "Users/{Name}/Appdata/LocalLow/{CompanyName}/{AppName}"
        */
        string path = Application.persistentDataPath + "/playerSave.sav";
        if(File.Exists(path)) {
            LoadStats();
        }
    }

    public void SavePlayer() {
        SaveSystem.SavePlayer(this);
    }

    void LoadStats() {
        PlayerStats data = SaveSystem.LoadPlayer();

        //Stats
        Runs = data.Runs;
        DepthLevel = data.DepthLevel;
        Wins = data.Wins;
        Deaths = data.Deaths;
        if(data.hasPlayed == 1) hasPlayed = true;
        else hasPlayed = false;
    }

    public void AddRun() {
        Runs++;
    }

    
    public void AddWins() {
        Wins++;
    }

    public void AddDeath() {
        Deaths++;
    }
    
    public void SetDepth(int value) {
        DepthLevel = value;
    }


    public void SetPlay(bool value) {
        hasPlayed = value;
    }
}
