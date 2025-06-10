using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private int _maxLevels = 3;
    [SerializeField] private bool _useRandomGen = false;

    [Title("Stats")]
    [SerializeField] public PlayerStats CurrentStats;
    [SerializeField] public GameSettings CurrentSettings;
    [ReadOnly] public int SelectedSave = -1;
    [ReadOnly] public bool HadPlayedSave1;
    [ReadOnly] public bool HadPlayedSave2;
    [ReadOnly] public bool HadPlayedSave3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        //Checks if save has been created after exiting game.
        string path1 = Application.persistentDataPath + "/playerSave1.sav";
        string path2 = Application.persistentDataPath + "/playerSave2.sav";
        string path3 = Application.persistentDataPath + "/playerSave3.sav";
        string gfxPath = Application.persistentDataPath + "/video.sav";
        if (File.Exists(path1)) HadPlayedSave1 = true;
        if (File.Exists(path2)) HadPlayedSave2 = true;
        if (File.Exists(path3)) HadPlayedSave3 = true;
        if (File.Exists(gfxPath)) LoadSettings();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayerData(this, SelectedSave);
    }

    public void SavePlayer(int index)
    {
        SaveSystem.SavePlayerData(this, index);
    }

    public void SaveSettings()
    {
        SaveSystem.SaveSettingsData(this);
    }

    public void LoadPlayer(int index)
    {
        LoadStats(index);
    }

    public void LoadSettings()
    {
        CurrentSettings = SaveSystem.LoadSettingsData();
        if (CurrentSettings == null) CurrentSettings = new();
    }

    void LoadStats(int index)
    {
        CurrentStats = SaveSystem.LoadPlayerData(index);
        SelectedSave = index;
        if (CurrentStats == null)
            CurrentStats = new();
    }

    public void AddRun()
    {
        CurrentStats.Runs++;
        SavePlayer();
    }

    public void AddWins()
    {
        CurrentStats.Wins++;
        SavePlayer();
    }

    public void AddDeath()
    {
        CurrentStats.Deaths++;
        SavePlayer();
    }

    public void AddDepth()
    {
        CurrentStats.DepthLevel++;
    }

    public void SetDepth(int value)
    {
        CurrentStats.DepthLevel = value;
    }

    public void SetPlay(bool value)
    {
        CurrentStats.hasPlayed = value ? 1 : 0;
    }

    public string GetNextLevel()
    {
        if (CurrentStats.DepthLevel >= _maxLevels)
            return "CronosLevel";

        if (!_useRandomGen)
            return "Level " + (CurrentStats.DepthLevel + 1);

        return "LevelScene";
    }
}
