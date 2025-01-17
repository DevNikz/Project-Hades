using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
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
    }

    [TitleGroup("References", "General Manager References", Alignment = TitleAlignments.Centered)]
        [TitleGroup("References/Augments")]
            [TabGroup("References/Augments/TabGroup", "Stackable Augments")]
                [SerializeField] private List<StackableAugment> stackableAugments = new();
            [TabGroup("References/Augments/TabGroup", "Stances")]
                [SerializeField] private List<UnlockableAugment> stanceAugments = new();
            [TabGroup("References/Augments/TabGroup", "Stance Sub Augments")]
                [SerializeField] private List<StanceSubAugment> stanceSubAugments = new();
            // [HorizontalGroup("References/Augments/TabGroup/Stances/Row", Width = 0.75f)]
                // [VerticalGroup("References/Augments/TabGroup/Stances/Row/Left"), LabelText("References")]
                // [SerializeReference] 
                // private List<AugmentScriptable> stanceAugments = new();
                
                // [VerticalGroup("References/Augments/TabGroup/Stances/Row/Right"), LabelText("Unlocked")]
                // [SerializeField] 
                // private List<bool> stanceAugmentActive = new();

    private void OnEnable() {
        // Make sure that the stackable augment counts match the actual references
        // while(stackableAugments.Count != stackableAugmentCounts.Count) {
        //     if(stackableAugmentCounts.Count > stackableAugments.Count)
        //         stackableAugmentCounts.Remove(stackableAugmentCounts[stackableAugmentCounts.Count - 1]);
        //     if(stackableAugmentCounts.Count < stackableAugments.Count)
        //         stackableAugmentCounts.Add(0);
        // }

        // Make sure that the stance augment bools match the actual references
        // while(stanceAugments.Count != stanceAugmentActive.Count) {
        //     if(stanceAugmentActive.Count > stanceAugments.Count)
        //         stanceAugmentActive.Remove(stanceAugmentActive[stanceAugmentActive.Count - 1]);
        //     if(stanceAugmentActive.Count < stanceAugments.Count)
        //         stanceAugmentActive.Add(false);
        // }
    }

    // [HorizontalGroup("References/Group")]
    // [VerticalGroup("References/Group/Left")]
    // [BoxGroup("References/Group/Left/Level")]
    // [LabelWidth(125)]
    // public int scrapCount;

    // [BoxGroup("References/Group/Left/Level")]
    // [LabelWidth(125)]
    // public int aggroCount;

    // [BoxGroup("References/Group/Left/Level")]
    // [LabelWidth(125)]
    // public int steelCount;

    // [BoxGroup("References/Group/Left/Level")]
    // [LabelWidth(125)]
    // public int heavyCount;

    // [BoxGroup("References/Group/Left/Level")]
    // [LabelWidth(125)]
    // public int vitalityCount;

    // [VerticalGroup("References/Group/Right")]
    // [BoxGroup("References/Group/Right/Player")]
    // [LabelWidth(125)]
    // public int playerScrapCount;

    // [BoxGroup("References/Group/Right/Player")]
    // [LabelWidth(125)]
    // [OnValueChanged("UpdatePlayerAggro")]
    // public int playerAggroCount;

    // [BoxGroup("References/Group/Right/Player")]
    // [LabelWidth(125)]
    // [OnValueChanged("UpdatePlayerSteel")]
    // public int playerSteelCount;

    // [BoxGroup("References/Group/Right/Player")]
    // [LabelWidth(125)]
    // [OnValueChanged("UpdatePlayerHeavy")]
    // public int playerHeavyCount;

    // [BoxGroup("References/Group/Right/Player")]
    // [LabelWidth(125)]
    // [OnValueChanged("UpdatePlayerVitality")]
    // public int playerVitalityCount;

    // [SerializeReference] private AugmentScriptable vitalityScriptable;
    // [SerializeReference] private AugmentScriptable aggroScriptable;
    // [SerializeReference] private AugmentScriptable steelScriptable;
    // [SerializeReference] private AugmentScriptable heavyScriptable;

    // [SerializeField] private bool isWaterUnlocked;
    // [SerializeField] private bool isWindUnlocked;
    // [SerializeField] private bool isFireUnlocked;

    // public bool Water{
    //     get { return this.isWaterUnlocked; }
    //     set { this.isWaterUnlocked = value; }
    // }

    // public bool Wind{
    //     get { return this.isWindUnlocked; }
    //     set { this.isWindUnlocked = value; }
    // }

    // public bool Fire{
    //     get { return this.isFireUnlocked; }
    //     set { this.isFireUnlocked = value; }
    // }

    // void Awake() {
    //     if(Instance == null) {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else Destroy(gameObject);

    //     isWaterUnlocked = false;
    //     isFireUnlocked = false;
    //     isWindUnlocked = false;
    // }
    
    // //Debug (Level)
    // [BoxGroup("Level Debug")]
    // [HorizontalGroup("Level Debug/Buttons")]
    // [VerticalGroup("Level Debug/Buttons/1")]
    // [Button("Add Scrap (Level)", ButtonSizes.Large)]
    // public void DAddScrap() {
    //     scrapCount++;
    // }

    // [VerticalGroup("Level Debug/Buttons/2")]
    // [Button("Add Aggro (Level)", ButtonSizes.Large)]
    // public void DAddAggro() {
    //     aggroCount++;
    // }

    // [VerticalGroup("Level Debug/Buttons/3")]
    // [Button("Add Steel (Level)", ButtonSizes.Large)]
    // public void DAddSteel() {
    //     steelCount++;
    // }

    // //Debug (Player)
    // [BoxGroup("Player Debug")]
    // [HorizontalGroup("Player Debug/Buttons2")]
    // [VerticalGroup("Player Debug/Buttons2/1")]
    // [Button("Add Scrap (Player)", ButtonSizes.Large)]
    // public void DPAddScrap() {
    //     playerScrapCount++;
    // }

    // [VerticalGroup("Player Debug/Buttons2/2")]
    // [Button("Add Aggro (Player)", ButtonSizes.Large)]
    // public void DPAddAggro() {
    //     //playerAggroCount++;
    //     PAddAggro(1);
    // }

    // [VerticalGroup("Player Debug/Buttons2/3")]
    // [Button("Add Vitality (Player)", ButtonSizes.Large)] //changed from steel to vita
    // public void DPAddVitality() {
    //     //playerSteelCount++;
    //     PAddVitality(1);
    // }

    // //DebugReset (Level)
    // [HorizontalGroup("Level Debug/Buttons3")]
    // [VerticalGroup("Level Debug/Buttons3/1")]
    // [Button("Reset Scrap (Level)", ButtonSizes.Large)]
    // public void ResetScrapL() {
    //     scrapCount = 0;
    // }

    // [VerticalGroup("Level Debug/Buttons3/2")]
    // [Button("Reset Aggro (Level)", ButtonSizes.Large)]
    // public void ResetAggroL() {
    //     aggroCount = 0;
    // }

    // [VerticalGroup("Level Debug/Buttons3/3")]
    // [Button("Reset Steel (Level)", ButtonSizes.Large)]
    // public void ResetSteelL() {
    //     steelCount = 0;
    // }

    // //DebugReset (Player)
    // [HorizontalGroup("Player Debug/Buttons4")]
    // [VerticalGroup("Player Debug/Buttons4/1")]
    // [Button("Reset Scrap (Player)", ButtonSizes.Large)]
    // public void ResetScrapP() {
    //     playerScrapCount = 0;
    // }

    // [VerticalGroup("Player Debug/Buttons4/2")]
    // [Button("Reset Aggro (Player)", ButtonSizes.Large)]
    // public void ResetAggroP() {
    //     playerAggroCount = 0;
    // }

    // [VerticalGroup("Player Debug/Buttons4/3")]
    // [Button("Reset Steel (Player)", ButtonSizes.Large)]
    // public void ResetSteelP() {
    //     playerSteelCount = 0;
    // }

    // //For Levels
    // public void AddScrap(int value) {
    //     scrapCount += value;
    // }

    // public void AddAggro(int value) {
    //     aggroCount += value;
    // }

    // public void AddSteel(int value) {
    //     steelCount += value;
    // }

    // public void AddHeavy(int value) {
    //     heavyCount += value;
    // }

    // public void AddVitality(int value) {
    //     vitalityCount += value;
    // }

    // //For Player
    // public void PAddScrap(int value) {
    //     playerScrapCount += value;
    // }

    // public void PAddAggro(int value) {
    //     playerAggroCount += value;
    //     UpdatePlayerAggro();
    // }

    // public void PAddSteel(int value) {
    //     playerSteelCount += value;
    //     UpdatePlayerSteel();
    // }

    // public void PAddHeavy(int value) {
    //     playerHeavyCount += value;
    //     UpdatePlayerHeavy();
    // }

    // public void PAddVitality(int value) {
    //     playerVitalityCount += value;
    //     UpdatePlayerVitality();
    // }

    // //Stuffs
    // public void UpdatePlayerAggro() {
    //     PlayerController.Instance.SetHealthDamage(playerAggroCount * (aggroScriptable.damageIncrease * 0.01f));
    // }

    // public void UpdatePlayerSteel() {
    //     PlayerController.Instance.SetTotalDefense(playerSteelCount * (steelScriptable.damageIncrease * 0.01f));
    // }

    // public void UpdatePlayerHeavy() {
    //     PlayerController.Instance.SetStunDamage(playerHeavyCount * (heavyScriptable.damageIncrease * 0.01f));
    // }

    // public void UpdatePlayerVitality() {
    //     PlayerController.Instance.SetTotalHealth(playerVitalityCount * vitalityScriptable.healthIncrease);
    // }
}
