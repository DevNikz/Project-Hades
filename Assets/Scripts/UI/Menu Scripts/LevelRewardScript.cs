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
    [SerializeField] GameObject[] augmentButtons;
    [SerializeField] EnemySpawner _enemySpawner;

    [SerializeField, ReadOnly] private List<AugmentScriptable> chosenAugments = new();

    [SerializeReference] private LootpoolScriptable lootpool;
    [SerializeField] private int maxRetryAugmentGenerate = 100;
    bool _loadedFromWaveEnd = false;

    [Button("Reload Augment Rewards", ButtonSizes.Large)]
    public void ReloadAugments(){
        if (lootpool == null)
        {
            Debug.LogWarning("[WARN]: Lootpool null");
            return;
        }
        
        lootpool.initialize();
        AssignSprites();
    }

    AugmentType choice = AugmentType.None;

    void Start(){
        ResetMenu();
    }

    private void ResetMenu()
    {
        Debug.Log(gameObject);
        gameObject.SetActive(false);
        ReloadAugments();

        choice = AugmentType.None;
        if (levelRewardMenu == null)
        {
            Debug.LogWarning("LevelRewardMenu null");
            return;
        }
        // levelRewardMenu.SetActive(false);
    }

    void AssignSprites()
    {
        if (ItemManager.Instance == null)
        {
            Debug.LogWarning("[WARN]: ItemManager null");
            return;
        }

        chosenAugments.Clear();
        foreach (var button in augmentButtons){
            AugmentScriptable chosenAugment = null;
            int currentAugmentGenRetries = 0;
            do
            {
                chosenAugment = ItemManager.Instance.getAugment(
                    lootpool.returnRandomizedItem()
                );

                if (chosenAugment != null)
                {
                    if (ItemManager.Instance.hasUnlocked(chosenAugment.augmentType))
                    {
                        chosenAugment = null;
                        continue;   
                    }

                    // Debug.Log($"ChosenAug: {chosenAugment.preReqAugment}");
                    // Debug.Log($"HasUnlocked: {ItemManager.Instance.hasUnlocked(chosenAugment.preReqAugment)}");

                    if (chosenAugment.preReqAugment != AugmentType.None && !ItemManager.Instance.hasUnlocked(chosenAugment.preReqAugment))
                    {
                        chosenAugment = null;
                        continue;
                    }
                }
            } while ((chosenAugment == null || chosenAugments.Contains(chosenAugment)) && (currentAugmentGenRetries++ <= maxRetryAugmentGenerate));

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
        Debug.Log(this);
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
            if (_loadedFromWaveEnd)
            {

                ContinueSpawning();
            }
            else
            {
                TransitionLevel();

            }
        }
        else if (chosenAugments.Count <= 0)
        {
            if (_loadedFromWaveEnd)
            {
                ContinueSpawning();

            }
            else
            {
                TransitionLevel();

            }
        }
    }

    public void TransitionLevel(){
        ResetMenu();
        SaveManager.Instance.AddDepth();
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(
            SaveManager.Instance.GetNextLevel()
        );
    }

    private void ContinueSpawning()
    {
        ResetMenu();
        _enemySpawner.SpawnWave();
    }

    public void Activate(bool loadedFromWaveEnd)
    {
        _loadedFromWaveEnd = loadedFromWaveEnd;
        levelRewardMenu.SetActive(true);
    }
}
