using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Button_Script : MonoBehaviour
{
    [SerializeField] private ASyncLoader asyncLoader;
    [SerializeField] private bool _isHubAvailable = false;

    [SerializeField] private string sceneToLoad = "Tutorial";
    public void OnStartClick(int saveIndex)
    {
        SaveManager.Instance.LoadPlayer(saveIndex);

        if (asyncLoader != null)
        {
            if (SaveManager.Instance.CurrentStats.hasPlayed == 1 && _isHubAvailable)
                sceneToLoad = "HubLevel";
            asyncLoader.LoadLevelBtn(sceneToLoad);
            SFXManager.Instance.SwitchAudio();
        }
        else
        {
            Debug.LogError("ASyncLoader is not assigned!");
        }
    }

    public void OnOptionsClick()
    {
        //load options
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
