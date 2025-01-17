using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Title("Stats")]

    [SerializeReference] public int Runs;
    [SerializeReference] public int DepthLevel;
    [SerializeReference] public int Wins;
    [SerializeReference] public int Deaths;
    [SerializeReference] public bool hasPlayed;

    [PropertySpace, Title("Debug")]
    [ReadOnly, SerializeReference] public bool save1;
    [ReadOnly, SerializeReference] public bool save2;
    [ReadOnly, SerializeReference] public bool save3;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        /* Save
            - persistentDataPath = "Users/{Name}/Appdata/LocalLow/{CompanyName}/{AppName}"
        */
        // string path = Application.persistentDataPath + "/playerSave.sav";
        // if(File.Exists(path)) {
        //     LoadStats();
        // }

        //Checks if save has been created after exiting game.
        string path1 = Application.persistentDataPath + "/playerSave1.sav";
        string path2 = Application.persistentDataPath + "/playerSave2.sav";
        string path3 = Application.persistentDataPath + "/playerSave3.sav";
        if(File.Exists(path1)) save1 = true;
        if(File.Exists(path2)) save2 = true;
        if(File.Exists(path3)) save3 = true;
        
    }

    public void SavePlayer(int index) {
        SaveSystem.SavePlayer(this, index);
    }

    public void LoadPlayer(int index) {
        LoadStats(index);
    }

    void LoadStats(int index) {
        PlayerStats data = SaveSystem.LoadPlayer(index);

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
