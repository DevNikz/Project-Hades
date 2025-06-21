using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonHighlightScript : MonoBehaviour
{
    private UnityEngine.UI.Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<UnityEngine.UI.Image>();
        image.enabled = false;
    }

    //Augment Tree
    public void OnButtonClick()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (button.name == "Vitality" ||
            button.name == "Aggro" ||
            button.name == "Steel" ||
            button.name == "Heavy" ||
            button.name == "Gaia" ||
            button.name == "Ouranos" || button.name == "Ouranos(Locked)" ||
            button.name == "Thalassa" || button.name == "Thalassa(Locked)" ||
            button.name == "Gehenna" || button.name == "Gehenna(Locked)")
        {
            image.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        }
        else
            image.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        gameObject.transform.position = button.transform.position;
        image.enabled = true;

    }

    //Reward Screen
    public void OnRewardScreenClick()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        image.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 250);
        gameObject.transform.position = button.transform.position;
        image.enabled = true;
    }

    public void ResetHighlight()
    {
        if(image != null)
        image.enabled = false;
    }
}
