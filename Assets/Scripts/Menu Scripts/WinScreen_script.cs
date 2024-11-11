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

    public void Reset() {
        PlayerController.Instance.gameObject.tag = "Player";
        PlayerController.Instance.RevertHealth();
        PlayerController.Instance.RevertMana();
    }
}
