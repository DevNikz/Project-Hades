using UnityEngine;

[System.Serializable]
public class GameSettings {
    public float masterVolume;
    public float musicVolume;
    public float gameplayVolume;
    public float menuVolume;
    public GameSettings()
    {
        masterVolume = 1f;
        musicVolume = 1f;
        gameplayVolume = 1f;
        menuVolume = 1f;
    }

    public GameSettings(SaveManager player)
    {
        masterVolume = player.CurrentSettings.masterVolume;
        musicVolume = player.CurrentSettings.musicVolume;
        gameplayVolume = player.CurrentSettings.gameplayVolume;
        menuVolume = player.CurrentSettings.menuVolume;
    }
}