using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AugmentTextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descText;

    public string hpnameField;
    public string hpDescField;

    public string atkNameField;
    public string atkDescField;

    public string defNameField;
    public string defDescField;

    public string stunNameField;
    public string stunDescField;

    public string earthNameField;
    public string earthDescField;

    public string fireNameField;
    public string fireDescField;

    public string waterNameField;
    public string waterDescField;

    public string windNameField;
    public string windDescField;

    private void Start()
    {
        nameText.text = "";
        descText.text = "";
    }

    public void OnButtonClick()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        switch (button.name)
        {
            //Stat augments
            case "Vitality":
                nameText.text = hpnameField;
                descText.text = hpDescField;
                break;
            case "Aggro":
                nameText.text = atkNameField;
                descText.text = atkDescField;
                break;
            case "Steel":
                nameText.text = defNameField;
                descText.text = defDescField;
                break;
            case "Heavy":
                nameText.text = stunNameField;
                descText.text = stunDescField;
                break;

            //Elemental augments
            case "Earth":
                nameText.text = earthNameField;
                descText.text = earthDescField;
                break;
            case "Water":
                nameText.text = waterNameField;
                descText.text = waterDescField;
                break;
            case "Wind":
                nameText.text = windNameField;
                descText.text = windDescField;
                break;
            case "Fire":
                nameText.text = fireNameField;
                descText.text = fireDescField;
                break;
        }


    }
}
