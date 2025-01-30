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
    private int totalRate = 0;
    public AugmentType returnRandomizedItem(){

        AugmentType chosenAugment = upperLimitAugmentMap[totalRate];

        int threshold = (int)(((float)UnityEngine.Random.Range(0, 100) / 100.0f) * (float)totalRate);
        foreach(var map in upperLimitAugmentMap){
            if(threshold > map.Key){
                chosenAugment = map.Value;
                break;
            }
        }

        return chosenAugment;
    }

    public void initialize(){
        totalRate = 0;
        upperLimitAugmentMap.Clear();

        // Compute total probability and map upperlimits
        int runningTotal = 0;
        foreach(var item in LootpoolItems){
            runningTotal += (int)(item.dropRate * 100);
            upperLimitAugmentMap.Add(runningTotal, item.Augment.augmentType);
            Debug.Log(runningTotal);
        }

        totalRate = runningTotal;
    }
}
