using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] public List<Slider> slider;

    void Awake()
    {
        LoadVolumes();
    }

    public void LoadVolumes()
    {
        slider[0].value = SaveManager.Instance.CurrentSettings.masterVolume;
        slider[1].value = SaveManager.Instance.CurrentSettings.musicVolume;
        slider[2].value = SaveManager.Instance.CurrentSettings.gameplayVolume;
        slider[3].value = SaveManager.Instance.CurrentSettings.menuVolume;
    }

    public void SetMasterVolume()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.SetMasterVolume(slider[0].value);
            SaveManager.Instance.CurrentSettings.masterVolume = slider[0].value;
        }
    }

    public void SetMusicVolume()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.SetMusicVolume(slider[1].value);
            SaveManager.Instance.CurrentSettings.musicVolume = slider[1].value;
        }
    }

    public void SetGameplayVolume()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.SetSFXVolume(slider[2].value);
            SaveManager.Instance.CurrentSettings.gameplayVolume = slider[2].value;
        }
    }

    public void SetMenuVolume()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.SetMenuVolume(slider[3].value);
            SaveManager.Instance.CurrentSettings.menuVolume = slider[3].value;
        }
    }
}
