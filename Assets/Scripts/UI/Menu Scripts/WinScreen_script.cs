using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen_script : MonoBehaviour
{
    public void OnButtonClick()
    {
        Reset();
        SceneManager.LoadScene("Title Screen");
    }

    public void Reset()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.gameObject.tag = "Player";
            PlayerController.Instance.RevertHealth();
            PlayerController.Instance.RevertMana();
        }

        if (RevampPlayerStateHandler.Instance != null)
        {
            RevampPlayerStateHandler.Instance.gameObject.tag = "Player";
            RevampPlayerStateHandler.Instance.ResetHealth();
            RevampPlayerStateHandler.Instance.ResetCharge();
        }
    }
}
