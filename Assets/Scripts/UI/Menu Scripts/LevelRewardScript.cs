using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelRewardScript : MonoBehaviour
{
    [SerializeField] private int _maxStanceCount = 2;
    [SerializeField] GameObject levelRewardMenu;
    [SerializeField] ButtonHighlightScript _selectHighlight;
    [SerializeField] GameObject[] augmentButtons;

    [SerializeField, ReadOnly] private List<AugmentScriptable> chosenAugments = new();

    [SerializeReference] private LootpoolScriptable lootpool;
    [SerializeField] private int maxRetryAugmentGenerate = 100;

    [SerializeField] private string _tutorialDialogueTag;

    bool _loadedFromWaveEnd = false;

    [Button("Reload Augment Rewards", ButtonSizes.Large)]
    public void ReloadAugments()
    {
        if (lootpool == null)
        {
            Debug.LogWarning("[WARN]: Lootpool null");
            return;
        }

        lootpool.initialize();
        AssignSprites();
    }

    AugmentType choice = AugmentType.None;

    void Start()
    {
        ResetMenu();
    }

    private void ResetMenu()
    {
        Time.timeScale = 1.0f;
        // Debug.Log(gameObject);
        gameObject.SetActive(false);
        _selectHighlight.ResetHighlight();
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
        foreach (var button in augmentButtons)
        {
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

                    if((
                        chosenAugment.augmentType == AugmentType.Earth || 
                        chosenAugment.augmentType == AugmentType.Water || 
                        chosenAugment.augmentType == AugmentType.Fire || 
                        chosenAugment.augmentType == AugmentType.Air) &&
                        ItemManager.Instance.UnlockedStanceCount >= _maxStanceCount
                    ){
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
        if (updater != null && updater.GetAugment() != null)
        {
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

    public void TransitionLevel()
    {
        ResetMenu();
        SaveManager.Instance.AddDepth();
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(
            SaveManager.Instance.GetNextLevel()
        );
    }

    private void ContinueSpawning()
    {
        ResetMenu();
        if(EnemySpawner.Instance != null)
            EnemySpawner.Instance.SpawnWave();
    }

    public void Activate(bool loadedFromWaveEnd)
    {
        Debug.Log("Reward Menu Activated");
        _loadedFromWaveEnd = loadedFromWaveEnd;

        if (!TryActivateDialogue())
            InternalActivate();
    }

    private void InternalActivate()
    {
        levelRewardMenu.SetActive(true);
        Time.timeScale = 0;
    }

    private bool TryActivateDialogue()
    {
        if (_tutorialDialogueTag.IsNullOrWhitespace()) return false;
        if (DialogueManager.Instance == null) return false;

        DialogueManager.Instance.StartDialogue(_tutorialDialogueTag, InternalActivate);
        _tutorialDialogueTag = "";
        return true;
    }
}
