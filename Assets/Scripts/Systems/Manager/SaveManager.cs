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
    [SerializeReference] private List<EnemyWaveSet> _nonRandomGenWavesets;

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

        SceneManager.sceneLoaded += OnSceneLoaded;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0: // Main Menu
            case 1: // Tutorial
            case 2: // Hub
                SetDepth(0);
                SavePlayer();
                break;
            default:
                CurrentStats.hasPlayed = 1;
                break;
        }
    }

    public EnemyWaveSet CurrentFloorWaveset
    {
        get
        {
            if (_nonRandomGenWavesets == null || _useRandomGen) return null;
            if (CurrentStats.DepthLevel > _nonRandomGenWavesets.Count) return null;
            if (CurrentStats.DepthLevel <= 0) return null;
            return _nonRandomGenWavesets[CurrentStats.DepthLevel - 1];
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayerData(this, SelectedSave);
    }


    public void DeleteSave(int selectedSave){
        SaveSystem.DeleteSave(selectedSave);
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
        SelectedSave = index;
        bool checkVal = false;
        switch (index)
        {
            case 1: checkVal = HadPlayedSave1; break;
            case 2: checkVal = HadPlayedSave2; break;
            case 3: checkVal = HadPlayedSave3; break;
        }

        if (checkVal)
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
        if (CurrentStats.DepthLevel > _maxLevels + 1)
            return "WinScreen";

        if (CurrentStats.DepthLevel > _maxLevels)
                return "CronosLevel";

        if (!_useRandomGen)
            return "Level " + (CurrentStats.DepthLevel);

        return "LevelScene";
    }
}
