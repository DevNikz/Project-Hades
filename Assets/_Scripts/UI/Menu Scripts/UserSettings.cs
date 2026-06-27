using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettings : MonoBehaviour
{
    [SerializeField] public List<Toggle> toggle;

    public bool isFullscreen;

    void OnEnable()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        toggle[0].isOn = SaveManager.Instance.CurrentSettings.screenMode != 0;
        toggle[1].isOn = SaveManager.Instance.CurrentSettings.vsync != 0;
        toggle[2].isOn = SaveManager.Instance.CurrentSettings.detail != 0;
        if(toggle[3] != null)
        toggle[3].isOn = SaveManager.Instance.CurrentSettings.difficulty != 0;
    }

    public void SetScreenMode()
    {
        if (GameSetting.Instance != null) GameSetting.Instance.SetScreenMode(toggle[0].isOn);
        SaveManager.Instance.CurrentSettings.screenMode = toggle[0].isOn ? 1 : 0;
        isFullscreen = toggle[0].isOn;
    }

    public void SetVsync()
    {
        if (GameSetting.Instance != null) GameSetting.Instance.SetVsync(toggle[1].isOn);
        SaveManager.Instance.CurrentSettings.vsync = toggle[1].isOn ? 1 : 0;
    }

    public void SetDetailMode()
    {
        if (GameSetting.Instance != null) GameSetting.Instance.SetDetailMode(toggle[2].isOn);
        SaveManager.Instance.CurrentSettings.detail = toggle[2].isOn ? 1 : 0;
    }
    public void SetDifficulty()
    {
        if (GameSetting.Instance != null) GameSetting.Instance.SetDifficulty(toggle[3].isOn);
        SaveManager.Instance.CurrentSettings.screenMode = toggle[3].isOn ? 1 : 0;
    }

}
