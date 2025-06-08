using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] public List<Slider> slider;


    public void SetMasterVolume()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.SetMasterVolume(slider[0].value);
    }

    public void SetMusicVolume()
    {
        if (SFXManager.Instance != null)
        {
            Debug.Log(slider[1].value);
            SFXManager.Instance.SetMusicVolume(slider[1].value);
        }
    }

    public void SetGameplayVolume()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.SetSFXVolume(slider[2].value);
    }

    public void SetMenuVolume()
    {

    }
}
