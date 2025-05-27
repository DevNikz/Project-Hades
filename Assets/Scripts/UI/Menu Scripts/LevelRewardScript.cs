using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelRewardScript : MonoBehaviour
{
    [SerializeField] GameObject levelRewardMenu;
    // [SerializeField] Sprite[] stanceSprites;
    [SerializeField] GameObject[] augmentButtons;
    // [SerializeField] GameObject stanceButton;
    // [SerializeField] private AugmentScriptable gaiaGear;
    // [SerializeField] private AugmentScriptable thalassaGear;
    // [SerializeField] private AugmentScriptable ouranosGear;
    // [SerializeField] private AugmentScriptable gehennaGear;

    [SerializeField] private List<AugmentScriptable> chosenAugments = new();

    [SerializeReference] private LootpoolScriptable lootpool;
    [SerializeField] private int maxRetryAugmentGenerate = 100;
    private int currentAugmentGenRetries = 0;

    [Button("Reload Augment Rewards", ButtonSizes.Large)]
    public void ReloadAugments(){
        lootpool.initialize();
        AssignSprites();
    }

    AugmentType choice = AugmentType.None;
    bool choiceMade = false;

    void Start(){
        ResetMenu();
    }

    private void ResetMenu(){
        lootpool.initialize();
        AssignSprites();

        currentAugmentGenRetries = 0;

        choice = AugmentType.None;
        choiceMade = false;
        levelRewardMenu.SetActive(false);
    }

    private void Update(){
        if(levelRewardMenu.activeInHierarchy){
            if (choiceMade){
                TransitionLevel();
            }
        }
    }

    void AssignSprites()
    {
        chosenAugments.Clear();

        foreach (var button in augmentButtons)
        {
            AugmentScriptable chosenAugment = null;
            
            currentAugmentGenRetries = 0;

            do {
                chosenAugment = ItemManager.Instance.getAugment(
                    lootpool.returnRandomizedItem()
                );

                if (chosenAugment != null)
                {
                    if (ItemManager.Instance.hasUnlocked(chosenAugment.augmentType))
                        chosenAugment = null;

                    if (chosenAugment.preReqAugment != AugmentType.None && !ItemManager.Instance.hasUnlocked(chosenAugment.preReqAugment))
                        chosenAugment = null;
                }
                
            } while ((chosenAugment == null || chosenAugments.Contains(chosenAugment)) && (currentAugmentGenRetries++ <= maxRetryAugmentGenerate));

            currentAugmentGenRetries = maxRetryAugmentGenerate;

            chosenAugments.Add(chosenAugment);
            button.GetComponent<AugmentIconUpdater>().SetAugment(chosenAugment);
        }
    }

    public void ButtonSelected()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        choice = AugmentType.None;

        AugmentIconUpdater updater = button.GetComponent<AugmentIconUpdater>();
        if(updater != null && updater.GetAugment() != null){
            choice = updater.GetAugment().augmentType;
        }
    }

    public void ChoiceMade()
    {
        if (choice != AugmentType.None)
        {
            ItemManager.Instance.AddAugment(choice);
            switch (choice)
            {
                case AugmentType.Water:
                    Broadcaster.Instance.AddBoolParam(AugmentMenuScript.WATER_UNLOCKED, EventNames.Augment.WATER_UNLOCKED, true);
                    break;
                case AugmentType.Air:
                    Broadcaster.Instance.AddBoolParam(AugmentMenuScript.WIND_UNLOCKED, EventNames.Augment.WIND_UNLOCKED, true);
                    break;
                case AugmentType.Fire:
                    Broadcaster.Instance.AddBoolParam(AugmentMenuScript.FIRE_UNLOCKED, EventNames.Augment.FIRE_UNLOCKED, true);
                    break;

            }
            choiceMade = true;
        }
    }

    public void TransitionLevel(){
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(
            SaveManager.Instance.GetNextLevel()
        );
        ResetMenu();
    }

    public void Activate()
    {
        levelRewardMenu.SetActive(true);
    }
}
