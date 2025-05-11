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
    [SerializeField] private Dictionary<int, AugmentType> upperLimitAugmentMap = new();
    [SerializeField, ReadOnly] private int totalRate = 0;
    public AugmentType returnRandomizedItem(){

        AugmentType chosenAugment = upperLimitAugmentMap[totalRate];

        int threshold = (int)(((float)UnityEngine.Random.Range(0, 100) / 100.0f) * (float)totalRate);
        // Debug.Log("Threshold: " + threshold);
        foreach(var map in upperLimitAugmentMap){
            // Debug.Log("Map Item: " + map.Key +", " + map.Value);
            // chosenAugment = map.Value;
            if(threshold < map.Key){
                chosenAugment = map.Value;
                break;
            }
        }

        // Debug.Log("Chosen Augment: " + chosenAugment);

        return chosenAugment;
    }

    public void initialize(){
        totalRate = 0;
        upperLimitAugmentMap.Clear();

        // Compute total probability and map upperlimits
        int runningTotal = 0;
        foreach(var item in LootpoolItems){
            runningTotal += (int)(item.dropRate * 100);
            if(item.Augment == null){
                Debug.LogWarning("[WARN]: Lootpool has null item");
                continue;
            }
            upperLimitAugmentMap.Add(runningTotal, item.Augment.augmentType);
            // Debug.Log(runningTotal);
        }

        totalRate = runningTotal;
    }
}
