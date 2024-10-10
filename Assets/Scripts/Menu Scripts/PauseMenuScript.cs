using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public static bool isPaused;


    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
       pauseMenu.SetActive(true);
       Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void OpenJournal()
    {
        Debug.Log("Open Journal");
    }

    public void OpenOptions()
    {
        Debug.Log("Open Options");
    }

    public void ResignGame()
    {
        SceneManager.LoadScene("Lose Screen");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
