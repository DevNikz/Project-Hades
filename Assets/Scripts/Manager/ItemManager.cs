using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [TitleGroup("References", "General Manager References", Alignment = TitleAlignments.Centered)]
    [HorizontalGroup("References/Group")]
    [VerticalGroup("References/Group/Left")]
    [BoxGroup("References/Group/Left/Level")]
    [LabelWidth(125)]
    public int scrapCount;

    [BoxGroup("References/Group/Left/Level")]
    [LabelWidth(125)]
    public int aggroCount;

    [BoxGroup("References/Group/Left/Level")]
    [LabelWidth(125)]
    public int steelCount;

    [BoxGroup("References/Group/Left/Level")]
    [LabelWidth(125)]
    public int heavyCount;

    [BoxGroup("References/Group/Left/Level")]
    [LabelWidth(125)]
    public int vitalityCount;

    [VerticalGroup("References/Group/Right")]
    [BoxGroup("References/Group/Right/Player")]
    [LabelWidth(125)]
    public int playerScrapCount;

    [BoxGroup("References/Group/Right/Player")]
    [LabelWidth(125)]
    [OnValueChanged("UpdatePlayerAggro")]
    public int playerAggroCount;

    [BoxGroup("References/Group/Right/Player")]
    [LabelWidth(125)]
    [OnValueChanged("UpdatePlayerSteel")]
    public int playerSteelCount;

    [BoxGroup("References/Group/Right/Player")]
    [LabelWidth(125)]
    [OnValueChanged("UpdatePlayerHeavy")]
    public int playerHeavyCount;

    [BoxGroup("References/Group/Right/Player")]
    [LabelWidth(125)]
    [OnValueChanged("UpdatePlayerVitality")]
    public int playerVitalityCount;

    bool isWaterUnlocked;
    bool isWindUnlocked;
    bool isFireUnlocked;

    public bool Water{
        get { return this.isWaterUnlocked; }
        set { this.isWaterUnlocked = value; }
    }

    public bool Wind{
        get { return this.isWindUnlocked; }
        set { this.isWindUnlocked = value; }
    }

    public bool Fire{
        get { return this.isFireUnlocked; }
        set { this.isFireUnlocked = value; }
    }

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        isWaterUnlocked = false;
        isFireUnlocked = false;
        isWindUnlocked = false;
    }
    
    //Debug (Level)
    [BoxGroup("Level Debug")]
    [HorizontalGroup("Level Debug/Buttons")]
    [VerticalGroup("Level Debug/Buttons/1")]
    [Button("Add Scrap (Level)", ButtonSizes.Large)]
    public void DAddScrap() {
        scrapCount++;
    }

    [VerticalGroup("Level Debug/Buttons/2")]
    [Button("Add Aggro (Level)", ButtonSizes.Large)]
    public void DAddAggro() {
        aggroCount++;
    }

    [VerticalGroup("Level Debug/Buttons/3")]
    [Button("Add Steel (Level)", ButtonSizes.Large)]
    public void DAddSteel() {
        steelCount++;
    }

    //Debug (Player)
    [BoxGroup("Player Debug")]
    [HorizontalGroup("Player Debug/Buttons2")]
    [VerticalGroup("Player Debug/Buttons2/1")]
    [Button("Add Scrap (Player)", ButtonSizes.Large)]
    public void DPAddScrap() {
        playerScrapCount++;
    }

    [VerticalGroup("Player Debug/Buttons2/2")]
    [Button("Add Aggro (Player)", ButtonSizes.Large)]
    public void DPAddAggro() {
        playerAggroCount++;
    }

    [VerticalGroup("Player Debug/Buttons2/3")]
    [Button("Add Steel (Player)", ButtonSizes.Large)]
    public void DPAddSteel() {
        playerSteelCount++;
    }

    //DebugReset (Level)
    [HorizontalGroup("Level Debug/Buttons3")]
    [VerticalGroup("Level Debug/Buttons3/1")]
    [Button("Reset Scrap (Level)", ButtonSizes.Large)]
    public void ResetScrapL() {
        scrapCount = 0;
    }

    [VerticalGroup("Level Debug/Buttons3/2")]
    [Button("Reset Aggro (Level)", ButtonSizes.Large)]
    public void ResetAggroL() {
        aggroCount = 0;
    }

    [VerticalGroup("Level Debug/Buttons3/3")]
    [Button("Reset Steel (Level)", ButtonSizes.Large)]
    public void ResetSteelL() {
        steelCount = 0;
    }

    //DebugReset (Player)
    [HorizontalGroup("Player Debug/Buttons4")]
    [VerticalGroup("Player Debug/Buttons4/1")]
    [Button("Reset Scrap (Player)", ButtonSizes.Large)]
    public void ResetScrapP() {
        playerScrapCount = 0;
    }

    [VerticalGroup("Player Debug/Buttons4/2")]
    [Button("Reset Aggro (Player)", ButtonSizes.Large)]
    public void ResetAggroP() {
        playerAggroCount = 0;
    }

    [VerticalGroup("Player Debug/Buttons4/3")]
    [Button("Reset Steel (Player)", ButtonSizes.Large)]
    public void ResetSteelP() {
        playerSteelCount = 0;
    }

    //For Levels
    public void AddScrap(int value) {
        scrapCount += value;
    }

    public void AddAggro(int value) {
        aggroCount += value;
    }

    public void AddSteel(int value) {
        steelCount += value;
    }

    public void AddHeavy(int value) {
        heavyCount += value;
    }

    public void AddVitality(int value) {
        vitalityCount += value;
    }

    //For Player
    public void PAddScrap(int value) {
        playerScrapCount += value;
    }

    public void PAddAggro(int value) {
        playerAggroCount += value;
    }

    public void PAddSteel(int value) {
        playerSteelCount += value;
    }

    public void PAddHeavy(int value) {
        playerHeavyCount += value;
    }

    public void PAddVitality(int value) {
        playerVitalityCount += value;
    }

    //Stuffs
    public void UpdatePlayerAggro() {
        PlayerController.Instance.SetHealthDamage(playerAggroCount * 5);
    }

    public void UpdatePlayerSteel() {
        PlayerController.Instance.SetTotalDefense(playerSteelCount * 5);
    }

    public void UpdatePlayerHeavy() {
        PlayerController.Instance.SetStunDamage(playerHeavyCount * 5);
    }

    public void UpdatePlayerVitality() {
        PlayerController.Instance.SetTotalHealth(playerVitalityCount * 5);
    }
}
