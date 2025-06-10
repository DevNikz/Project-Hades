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
        toggle[1].isOn = SaveManager.Instance.CurrentSettings.difficulty != 0;
        toggle[2].isOn = SaveManager.Instance.CurrentSettings.detail != 0;
    }

    public void SetScreenMode()
    {
        if (GameSetting.Instance != null) GameSetting.Instance.SetScreenMode(toggle[0].isOn);
        SaveManager.Instance.CurrentSettings.screenMode = toggle[0].isOn ? 1 : 0;
        isFullscreen = toggle[0].isOn;
    }

    public void SetDifficulty()
    {
        if (GameSetting.Instance != null) GameSetting.Instance.SetDifficulty(toggle[1].isOn);
        SaveManager.Instance.CurrentSettings.screenMode = toggle[1].isOn ? 1 : 0;
    }

    public void SetDetailMode()
    {
        if (GameSetting.Instance != null) GameSetting.Instance.SetDetailMode(toggle[2].isOn);
        SaveManager.Instance.CurrentSettings.detail = toggle[2].isOn ? 1 : 0;
    }


}
