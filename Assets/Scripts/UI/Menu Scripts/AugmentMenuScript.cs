using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentMenuScript : MonoBehaviour
{
    [SerializeField] GameObject augmentMenu;
    [SerializeField] public Sprite lockedSprite;

    [SerializeReference] bool debug = false;

    [Serializable]
    public class UnlockableAugment
    {
        [HorizontalGroup("Row")]
        [VerticalGroup("Row/Left")][SerializeReference] public AugmentScriptable Augment;
        [VerticalGroup("Row/Buttons"), HorizontalGroup("Row")][SerializeField] public GameObject button;
    }

    [SerializeField] private List<UnlockableAugment> unlockableAugments = new();

    [TitleGroup("Augment Text Counters")]
    [SerializeField] private TextMeshProUGUI vitalityCount;
    [SerializeField] private TextMeshProUGUI aggroCount;
    [SerializeField] private TextMeshProUGUI steelCount;
    [SerializeField] private TextMeshProUGUI heavyCount;

    protected static bool isKeyHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        augmentMenu.SetActive(false);

        vitalityCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Vitality).ToString();
        aggroCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Aggro).ToString();
        steelCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Steel).ToString();
        heavyCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Heavy).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        DoAction();

        if (debug)
        {
            augmentMenu.SetActive(true);

            vitalityCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Vitality).ToString();
            aggroCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Aggro).ToString();
            steelCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Steel).ToString();
            heavyCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Heavy).ToString();
        }
        else augmentMenu.SetActive(false);
    }

    void DoAction() {
        if (Input.GetKey(KeyCode.T)) {
            isKeyHeld = true;
            debug = true;
        }
        else if (Input.GetKeyUp(KeyCode.T)) {
            isKeyHeld = false;
            debug = false;
        }
    }

    public void UnlockAugment(AugmentType augmentType)
    {
        foreach(var augment in unlockableAugments)
        {
            //Class > Scriptable > Enum
            if(augment.Augment.augmentType == augmentType)
            {
                augment.button.GetComponent<Image>().sprite = augment.Augment.augmentIcon;
            }
        }
    }
}
