using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AugmentTextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descText;

    public string hpNameField;
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

    public void OnButtonClick(AugmentScriptable augmentInfo)
    {
        nameText.text = augmentInfo.augmentName;
        descText.text = augmentInfo.augmentDescription;
    }
}
