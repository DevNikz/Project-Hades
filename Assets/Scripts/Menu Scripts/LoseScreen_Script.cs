using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen_Script : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
