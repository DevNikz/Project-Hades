using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Button_Script : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("Level 1");
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
