using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Button_Script : MonoBehaviour
{
    [SerializeField] private ASyncLoader asyncLoader;

    [SerializeField] private string sceneToLoad = "Tutorial";
    public void OnStartClick()
    {
        if (asyncLoader != null)
        {
            asyncLoader.LoadLevelBtn(sceneToLoad);
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
