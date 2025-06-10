using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deathOverlay;

    private static bool isPaused;

    public static bool isPausedCheck {
        get { return isPaused; }
        set { isPaused = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsMenuScript.isOptionsCheck == false )
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
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

    public void OpenOptions()
    {
        Debug.Log("Open Options");
    }

    public void ResignGame()
    {
        if (RevampPlayerStateHandler.Instance != null)
        {
            RevampPlayerStateHandler.Instance.ReceiveDamage(DamageType.Physical, 999);
            // RevampPlayerStateHandler.Instance.CurrentState = EntityState.Dead;
            // RevampPlayerStateHandler.Instance.gameObject.tag = "Player(Dead)";
        }

        deathOverlay.GetComponent<LoseScreen_Script>().Defeat();
        
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ExitGame()
    {
        // ResumeGame(); //Should fix the menu thingy
        // SceneManager.LoadScene("Title Screen");

        //Maybe put save here
        ResumeGame();
        Application.Quit();
    }

    public void PlayHoverSFX()
    {
        SFXManager.Instance.PlaySFX("HoverUI");
    }

    public void PlayClickSFX()
    {
        SFXManager.Instance.PlaySFX("ConfirmSFX");
    }
}
