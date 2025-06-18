using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
#region References
    public static ItemManager Instance;
    //[SerializeField] private RevampPlayerStateHandler _player;

    // Definitions for ease of tracking
    [Serializable] public class StackableAugment{
        [HorizontalGroup("Row")]
            [VerticalGroup("Row/Left")][SerializeReference] public AugmentScriptable Augment;
            
            [VerticalGroup("Row/Right"), HorizontalGroup("Row", Width = 0.2f)][SerializeField] public int Count;
    }
    [Serializable] public class UnlockableAugment{
        [HorizontalGroup("Row")]
            [VerticalGroup("Row/Left")][SerializeReference] public AugmentScriptable Augment;
            
            [VerticalGroup("Row/Right"), HorizontalGroup("Row", Width = 0.2f)][SerializeField] public bool Unlocked;
    }
    [Serializable] public class StanceSubAugment : UnlockableAugment{
        [VerticalGroup("Row/StanceIndex"), HorizontalGroup("Row", Width = 0.2f)][SerializeField] public int StanceIndex;
        [VerticalGroup("Row/StanceSubtree"), HorizontalGroup("Row", Width = 0.2f)][SerializeField] public int StanceSubtree;
        [VerticalGroup("Row/RefElement"), HorizontalGroup("Row", Width = 0.2f)][SerializeField] public Elements RefElement;
    }

    // References
    [TitleGroup("References", "General Manager References", Alignment = TitleAlignments.Centered)]
        [TitleGroup("References/Augments")]
            [TabGroup("References/Augments/TabGroup", "Stackable Augments")]
                [SerializeField] private List<StackableAugment> stackableAugments = new();
            [TabGroup("References/Augments/TabGroup", "Stances")]
                [SerializeField] private List<UnlockableAugment> stanceAugments = new();
            [TabGroup("References/Augments/TabGroup", "Stance Sub Augments")]
                [SerializeField] private List<StanceSubAugment> stanceSubAugments = new();

    // Debug (Player)
    [BoxGroup("Player Debug")]
        [HorizontalGroup("Player Debug/Button")]
            [VerticalGroup("Player Debug/Button/Right")]
            [Button("Add/Unlock Chosen Augment", ButtonSizes.Medium)]
            public void DebugAddAugment(){
                AddAugment(debugAugmentType);
            }

            [VerticalGroup("Player Debug/Button/Right")]
            [Button("Add/Remove Chosen Augment", ButtonSizes.Medium)]
            public void DebugRemoveAugment(){
                RemoveAugment(debugAugmentType);
            }

            [VerticalGroup("Player Debug/Button/Left")]
            [SerializeField, LabelText("Type")] private AugmentType debugAugmentType;

    // Helper Fields
    private Dictionary<AugmentType, int> augmentIndexRef = new Dictionary<AugmentType, int>();
#endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        // Make sure that all augment indecies are correct
        augmentIndexRef.Clear();

        int stackableAugmentCount = stackableAugments.Count;
        int stanceAugmentCount = stanceAugments.Count;
        int stanceSubAugmentCount = stanceSubAugments.Count;

        for (int i = 0; i < stackableAugmentCount; i++)
            augmentIndexRef.Add(stackableAugments[i].Augment.augmentType, i);
        for (int i = 0; i < stanceAugmentCount; i++)
            augmentIndexRef.Add(stanceAugments[i].Augment.augmentType, i);
        for (int i = 0; i < stanceSubAugmentCount; i++)
            augmentIndexRef.Add(stanceSubAugments[i].Augment.augmentType, i);
    }

    private void ClearAugmentsExceptStance(AugmentType type)
    {
        // Make aure all stackable augment counts are reset to 0
        foreach (var augment in stackableAugments)
            augment.Count = 0;

        // Remove unlock of all other augments as both stance and stance sub augments
        foreach (var augment in stanceAugments)
            augment.Unlocked = false;
        foreach (var augment in stanceSubAugments)
            augment.Unlocked = false;

        // Unlock the the correct stance
        AddAugment(type);
    }
    
    public AugmentType HubSelectedStance = AugmentType.None;
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0: // Main Menu
                ClearAugmentsExceptStance(AugmentType.Earth);
                break;
            case 1: // Tutorial
            case 2: // Hub
                ClearAugmentsExceptStance(HubSelectedStance);
                break;
        }
    }

    public bool Earth
    {
        get { return stanceAugments[0].Unlocked; }
        set { stanceAugments[0].Unlocked = value; }
    }
    public bool Water{
        get { return stanceAugments[1].Unlocked; }
        set { stanceAugments[1].Unlocked = value; }
    }

    public bool Wind{
        get { return stanceAugments[2].Unlocked; }
        set { stanceAugments[2].Unlocked = value; }
    }

    public bool Fire{
        get { return stanceAugments[3].Unlocked; }
        set { stanceAugments[3].Unlocked = value; }
    }

    public AugmentScriptable getAugment(AugmentType type){
        StackableAugment stackableAugment = getStackableAugment(type);
        if(stackableAugment != null)
            return stackableAugment.Augment;

        UnlockableAugment unlockableAugment = getUnlockableAugment(type);
        if(unlockableAugment != null)
            return unlockableAugment.Augment;
        
        return null;
    }

    public int getAugmentCount(AugmentType type){
        StackableAugment stackbleAugment = getStackableAugment(type);
        if (stackbleAugment == null) return -1;

        return stackbleAugment.Count;
    }

    public bool hasUnlocked(AugmentType type){
        UnlockableAugment unlockableAugment = getUnlockableAugment(type);
        if(unlockableAugment == null) return false;

        return unlockableAugment.Unlocked;
    }

    private StackableAugment getStackableAugment (AugmentType type){
        if(!augmentIndexRef.ContainsKey(type))
            return null;

        if(stackableAugments.Count > augmentIndexRef[type]){
            if(stackableAugments[augmentIndexRef[type]].Augment.augmentType == type)
                return stackableAugments[augmentIndexRef[type]];
        }

        return null;
    
    }
    
    private UnlockableAugment getUnlockableAugment (AugmentType type){
        if(!augmentIndexRef.ContainsKey(type))
            return null;

        if(stanceAugments.Count > augmentIndexRef[type]){
            if(stanceAugments[augmentIndexRef[type]].Augment.augmentType == type)
                return stanceAugments[augmentIndexRef[type]];
        }

        if(stanceSubAugments.Count > augmentIndexRef[type]){
            if(stanceSubAugments[augmentIndexRef[type]].Augment.augmentType == type)
                return stanceSubAugments[augmentIndexRef[type]];
        }

        return null;
    }

    //For Player
    public void AddAugment(AugmentType type, int amount = 1) { 
        if(amount <= 0) return;

        StackableAugment stackAugment = getStackableAugment(type);
        if(stackAugment != null){
            stackAugment.Count += amount;

            if (stackAugment.Augment.augmentType == AugmentType.Vitality)
            {
                UpdatePlayerVitality();
                // if(!_player)
                //     _player = GameObject.Find("Player").GetComponent<RevampPlayerStateHandler>();
                //_player.HealHealth(stackAugment.Augment.augmentPower);
                RevampPlayerStateHandler.Instance.HealHealth(stackAugment.Augment.augmentPower);
            }
            else
            {
                ToggleValidAugments(Elements.None);
            }
            return;
        }

        UnlockableAugment unlockAugment = getUnlockableAugment(type);
        if(unlockAugment != null){
            unlockAugment.Unlocked = true;
            // unlockAugment.Augment.OnActivate();
            ToggleValidAugments(Elements.None);
            return;
        }
        
        Debug.Log($"Added {amount} {type} Augment/s");
    }

    public void RemoveAugment(AugmentType type, int amount = 1) { 
        if(amount <= 0) return;

        StackableAugment stackAugment = getStackableAugment(type);
        if(stackAugment != null){
            // if(stackAugment.Count >= 1)
            //     stackAugment.Augment.OnDeactivate();
            stackAugment.Count -= amount;
            if(stackAugment.Count < 0)
                stackAugment.Count = 0;
            ToggleValidAugments(Elements.None);
            return;
        }

        UnlockableAugment unlockAugment = getUnlockableAugment(type);
        if(unlockAugment != null){
            unlockAugment.Unlocked = false;
            ToggleValidAugments(Elements.None);
            // unlockAugment.Augment.OnDeactivate();
            return;
        }
        
    }

    public void ClearAugment(AugmentType type){
        StackableAugment stackAugment = getStackableAugment(type);
        if(stackAugment != null){
            for(int i = 0; i < stackAugment.Count; i++)
                stackAugment.Augment.OnDeactivate();
            stackAugment.Count = 0;
            return;
        }

        UnlockableAugment unlockAugment = getUnlockableAugment(type);
        if(unlockAugment != null){
            unlockAugment.Unlocked = false;
            unlockAugment.Augment.OnDeactivate();
            return;
        }
    }

    public void ToggleValidAugments(Elements activeStance){
        foreach(StackableAugment augment in stackableAugments){
            if(augment.Augment.augmentType == AugmentType.Vitality){
                UpdatePlayerVitality();
                return;
            }

            if(augment.Count >= 1){
                if(!augment.Augment.IsActive)
                    augment.Augment.OnActivate();
            } else {
                if(augment.Augment.IsActive)
                    augment.Augment.OnDeactivate();
            } 
        }

        foreach(UnlockableAugment augment in stanceAugments){
            if(augment.Unlocked){
                if(!augment.Augment.IsActive)
                    augment.Augment.OnActivate();
            } else {
                if(augment.Augment.IsActive)
                    augment.Augment.OnDeactivate();
            }
        }

        if(activeStance == Elements.None)
            return;

        foreach(StanceSubAugment augment in stanceSubAugments){
            if(augment.RefElement == activeStance){
                if(augment.Unlocked){
                    if(!augment.Augment.IsActive)
                        augment.Augment.OnActivate();
                } else {
                    if(augment.Augment.IsActive)
                        augment.Augment.OnDeactivate();
                }
            } else {
                if(augment.Augment.IsActive)
                    augment.Augment.OnDeactivate();
            }
        }

        
    }

    public void PAddAggro(int value) {
        AddAugment(AugmentType.Aggro, value);
        UpdatePlayerAggro();
    }

    public void PAddSteel(int value) {
        AddAugment(AugmentType.Steel, value);
        UpdatePlayerSteel();
    }

    public void PAddHeavy(int value) {
        AddAugment(AugmentType.Heavy, value);
        UpdatePlayerHeavy();
    }

    public void PAddVitality(int value) {
        AddAugment(AugmentType.Vitality, value);
        UpdatePlayerVitality();
    }

    //Stuffs
    public void UpdatePlayerAggro() {
        StackableAugment augment = getStackableAugment(AugmentType.Aggro);
        AugmentScriptable scriptable = augment.Augment;
        int count = augment.Count;

        PlayerController.Instance.SetHealthDamage(count * (scriptable.augmentPower * 0.01f));
    }

    public void UpdatePlayerSteel() {
        StackableAugment augment = getStackableAugment(AugmentType.Steel);
        AugmentScriptable scriptable = augment.Augment;
        int count = augment.Count;
        

        PlayerController.Instance.SetTotalDefense(count * (scriptable.augmentPower * 0.01f));
    }

    public void UpdatePlayerHeavy() {
        StackableAugment augment = getStackableAugment(AugmentType.Heavy);
        AugmentScriptable scriptable = augment.Augment;
        int count = augment.Count;

        PlayerController.Instance.SetStunDamage(count * (scriptable.augmentPower * 0.01f));
    }

    public void UpdatePlayerVitality() {
        StackableAugment augment = getStackableAugment(AugmentType.Vitality);
        AugmentScriptable scriptable = augment.Augment;
        int count = augment.Count;

        PlayerController.Instance?.SetBonusHealth(count * scriptable.augmentPower);
        RevampPlayerStateHandler.Instance?.SetBonusHealth(count * scriptable.augmentPower);
    }

    private void Update() {
        // Play the Active Effect of all augments, skipping over disable augments
        foreach (var augment in stackableAugments){
            if(augment.Augment.IsActive)
                augment.Augment.ActiveEffect(augment.Count);
        }
        foreach (var augment in stanceAugments){
            if(augment.Augment.IsActive)
                augment.Augment.ActiveEffect();
        }
        foreach (var augment in stanceSubAugments){
            if(augment.Augment.IsActive)
                augment.Augment.ActiveEffect();
        }
    }
}
