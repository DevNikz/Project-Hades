using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    [Button("Reload Augment Rewards", ButtonSizes.Large)]
    public void ReloadAugments(){
        AssignSprites();
    }

    AugmentType choice = AugmentType.None;
    bool choiceMade = false;

    public string nextLevel = null;

    void Start()
    {
        ResetMenu();
    }

    private void ResetMenu(){
        lootpool.initialize();
        AssignSprites();

        choice = AugmentType.None;
        choiceMade = false;
        levelRewardMenu.SetActive(false);
    }

    private void Update()
    {
        if(levelRewardMenu.activeInHierarchy)
        {
            //Time.timeScale = 0.0f;

            if (choiceMade && nextLevel != null)
            {
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
            
            do{
                chosenAugment = ItemManager.Instance.getAugment(
                    lootpool.returnRandomizedItem()
                );

                if(ItemManager.Instance.hasUnlocked(chosenAugment.augmentType))
                    chosenAugment = null;
                
            } while ((chosenAugment == null || chosenAugments.Contains(chosenAugment)) && (maxRetryAugmentGenerate-- > 0));

            // Debug.Log(chosenAugments.Contains(chosenAugment));

            chosenAugments.Add(chosenAugment);
            button.GetComponent<AugmentIconUpdater>().SetAugment(chosenAugment);
        }

        // if(!ItemManager.Instance.Water)
        // {
        //     stanceButton.GetComponent<AugmentIconUpdater>().SetAugment(thalassaGear);
        //     stanceButton.GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[0];
        //     stanceButton.name = stanceSprites[0].name;  
        // }
        // else if(!ItemManager.Instance.Wind)
        // {
        //     stanceButton.GetComponent<AugmentIconUpdater>().SetAugment(ouranosGear);
        //     stanceButton.GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[1];
        //     stanceButton.name = stanceSprites[1].name;
        // }
        // else if (!ItemManager.Instance.Fire)
        // {
        //     stanceButton.GetComponent<AugmentIconUpdater>().SetAugment(gehennaGear);
        //     stanceButton.GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[2];
        //     stanceButton.name = stanceSprites[2].name;
        // }
    }

    public void ButtonSelected()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        choice = AugmentType.None;

        AugmentIconUpdater updater = button.GetComponent<AugmentIconUpdater>();
        if(updater != null){
            choice = updater.GetAugment().augmentType;
        }

        Debug.Log(choice);
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
        else
            Debug.Log("bad");
    }

    public void TransitionLevel()
    {
        //Time.timeScale = 1f;
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(nextLevel);
        //SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
        ResetMenu();
        //StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        levelRewardMenu.SetActive(false);
        yield return null;
    }

    public void Activate()
    {
        levelRewardMenu.SetActive(true);
    }
}
