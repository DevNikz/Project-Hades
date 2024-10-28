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

    public void OnButtonClick()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if(button.name == "Vitality" ||
            button.name == "Aggro" ||
            button.name == "Steel" ||
            button.name == "Heavy" ||
            button.name == "Earth" ||
            button.name == "Wind" ||
            button.name == "Water" ||
            button.name == "Fire")
        {
            image.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        }
        else
            image.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        gameObject.transform.position = button.transform.position;
        image.enabled = true;

    }
}
