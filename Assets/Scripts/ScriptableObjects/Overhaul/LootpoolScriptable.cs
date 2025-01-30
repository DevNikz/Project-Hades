using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "Lootpool", menuName = "ProjectHades/General/Lootpool", order = 2)]
[InlineEditor]
public class LootpoolScriptable : ScriptableObject
{
    [Serializable] public class LootpoolItemDroprate {
        [HorizontalGroup("Row")]
        [VerticalGroup("Row/Left")]
        [SerializeReference] public AugmentScriptable Augment;
        [VerticalGroup("Row/Right"), HorizontalGroup("Row", Width = 0.2f)]
        [SerializeField] public float dropRate = 0.0f;
    }

    [SerializeField] private List<LootpoolItemDroprate> LootpoolItems = new();
    [SerializeField] private Dictionary<float, AugmentType> upperLimitAugmentMap = new();
    private float totalRate = 0.0f;
    private bool isInitialized = false;
    
    public AugmentType returnRandomizedItem(){
        if(!isInitialized) initializeLootpool();

        AugmentType chosenAugment = upperLimitAugmentMap[totalRate];

        float threshold = ((float)UnityEngine.Random.Range(0, 100) / 100.0f) * totalRate;
        foreach(var map in upperLimitAugmentMap){
            if(threshold > map.Key){
                chosenAugment = map.Value;
                break;
            }
        }

        return chosenAugment;
    }

    private void initializeLootpool(){
        totalRate = 0.0f;
        upperLimitAugmentMap.Clear();

        // Compute total probability and map upperlimits
        float runningTotal = 0.0f;
        foreach(var item in LootpoolItems){
            runningTotal += item.dropRate;
            upperLimitAugmentMap.Add(runningTotal, item.Augment.augmentType);
        }

        totalRate = runningTotal;
        isInitialized = true;
    }
}
