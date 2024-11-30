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

    public void OnButtonClick(AugmentIconUpdater augmentUpdater)
    {
        AugmentScriptable augmentInfo = augmentUpdater.GetAugment();
        if(augmentInfo == null) return;

        nameText.text = augmentInfo.augmentName;
        descText.text = augmentInfo.augmentDescription;
    }
}
