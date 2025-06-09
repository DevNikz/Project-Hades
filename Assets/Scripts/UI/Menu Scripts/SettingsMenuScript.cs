using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuScript : MonoBehaviour
{
    //ref to slider
    //ref to mixer

    private static bool isOptions;
    public static bool isOptionsCheck
    {
        get { return isOptions; }
        set { isOptions = value; }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptions) Close();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        SetOptions(false);
        SaveManager.Instance.SaveSettings();
    }

    public bool checkOptions()
    {
        return isOptions;
    }

    public void SetOptions(bool value)
    {
        isOptions = value;
    }

    /*func for slider()
    {
        slider to mixer shit
    }
     */
}
