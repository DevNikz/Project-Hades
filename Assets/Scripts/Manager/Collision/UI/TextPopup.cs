using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class PopupTextController : MonoBehaviour
{

    public TextMeshProUGUI popupText;

    public void ShowPopupText(string message)
    {
        if (popupText != null)
        {
            popupText.text = message;        
            popupText.gameObject.SetActive(true); 
        }
        else
        {
            Debug.LogWarning("Popup Text is not assigned.");
        }
    }

    // Hide the popup text
    public void HidePopupText()
    {
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false); 
        }
    }
}


