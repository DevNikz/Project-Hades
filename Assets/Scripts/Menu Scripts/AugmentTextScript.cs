using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AugmentTextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descText;

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
                nameText.text = "HP";
                descText.text = "Increase max health";
                break;
            case "Aggro":
                nameText.text = "Attack";
                descText.text = "Increase basic attack";
                break;
            case "Steel":
                nameText.text = "Defense";
                descText.text = "Decrease damage received";
                break;
            case "Heavy":
                nameText.text = "Stun";
                descText.text = "Increase stun damage";
                break;

            //Elemental augments
            case "Earth":
                nameText.text = "Gaia";
                descText.text = "Increase stagger damage";
                break;
            case "Water":
                nameText.text = "Thalassa";
                descText.text = "Increase attack range";
                break;
            case "Air":
                nameText.text = "Ouranos";
                descText.text = "Increase attack speed";
                break;
            case "Fire":
                nameText.text = "Gehenna";
                descText.text = "Increase basic attack damage";
                break;
        }


    }
}
