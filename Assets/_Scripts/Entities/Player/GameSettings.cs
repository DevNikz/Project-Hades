using UnityEngine;

[System.Serializable]
public class GameSettings {
    public float masterVolume;
    public float musicVolume;
    public float gameplayVolume;
    public float menuVolume;
    public int screenMode;
    public int vsync;
    public int difficulty;
    public int detail;
    public GameSettings()
    {
        masterVolume = 1f;
        musicVolume = 1f;
        gameplayVolume = 1f;
        menuVolume = 1f;
        screenMode = 1;
        vsync = 1;
        difficulty = 0;
        detail = 1;
    }

    public GameSettings(SaveManager player)
    {
        masterVolume = player.CurrentSettings.masterVolume;
        musicVolume = player.CurrentSettings.musicVolume;
        gameplayVolume = player.CurrentSettings.gameplayVolume;
        menuVolume = player.CurrentSettings.menuVolume;
        screenMode = player.CurrentSettings.screenMode;
        vsync = player.CurrentSettings.vsync;
        difficulty = player.CurrentSettings.difficulty;
        detail = player.CurrentSettings.detail;
    }
}