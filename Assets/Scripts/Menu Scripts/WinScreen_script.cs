using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen_script : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
