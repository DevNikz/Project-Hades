using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AugmentMenuScript : MonoBehaviour
{
    [SerializeField] GameObject augmentMenu;
    [SerializeField] Sprite lockedSprite;

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

    private bool inHub = false;

    // Start is called before the first frame update
    void Start()
    {
        augmentMenu.SetActive(false);

        vitalityCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Vitality).ToString();
        aggroCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Aggro).ToString();
        steelCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Steel).ToString();
        heavyCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Heavy).ToString();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Tutorial" || scene.name == "Level 1")
            ChangeHubStance();
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

            if(SceneManager.GetActiveScene().name == "HubLevel")
            {
                inHub = true;
                ChangeHubStance();
            }
                
        }
        else augmentMenu.SetActive(false);
        inHub = false;
    }

    void DoAction() {
        if (Input.GetKeyDown(KeyCode.T)) {
            if(debug) Time.timeScale = 1.0f;
            else Time.timeScale = 0.0f;
            debug = !debug;
        }
    }

    public void ChangeAugment(AugmentType augmentType, bool unlock)
    {
        foreach(var augment in unlockableAugments)
        {
            //Class > Scriptable > Enum
            if(augment.Augment.augmentType == augmentType && unlock)
            {
                augment.button.GetComponent<Image>().sprite = augment.Augment.augmentIcon;
            }
            else if (augment.Augment.augmentType == augmentType & !unlock)
            {
                augment.button.GetComponent<Image>().sprite = lockedSprite;
            }
        }
    }

    void ChangeHubStance()
    {
        switch (ItemManager.Instance.HubSelectedStance)
        {
            case EStance.Earth:
                ChangeAugment(AugmentType.Earth, true);
                if(inHub)
                {
                    ChangeAugment(AugmentType.Fire, false);
                    ChangeAugment(AugmentType.Water, false);
                    ChangeAugment(AugmentType.Air, false);
                }
                break;
            case EStance.Fire:
                ChangeAugment(AugmentType.Fire, true);
                if (inHub)
                {
                    ChangeAugment(AugmentType.Earth, false);
                    ChangeAugment(AugmentType.Water, false);
                    ChangeAugment(AugmentType.Air, false);
                }
                break;
            case EStance.Water:
                ChangeAugment(AugmentType.Water, true);
                if (inHub)
                {
                    ChangeAugment(AugmentType.Fire, false);
                    ChangeAugment(AugmentType.Earth, false);
                    ChangeAugment(AugmentType.Air, false);
                }
                break;
            case EStance.Air:
                ChangeAugment(AugmentType.Air, true);
                if (inHub)
                {
                    ChangeAugment(AugmentType.Fire, false);
                    ChangeAugment(AugmentType.Water, false);
                    ChangeAugment(AugmentType.Earth, false);
                }
                break;
        }
    }
}
