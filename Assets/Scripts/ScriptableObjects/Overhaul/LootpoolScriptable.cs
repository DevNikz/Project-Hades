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
        [SerializeReference] public AugmentScriptable Augment;
        [SerializeField] public float dropRate;
    }

    [SerializeField] public List<LootpoolItemDroprate> LootpoolItems = new();
    private float totalRate = 0;
    public AugmentType returnRandomizedItem(){
        // int chosenValue = UnityEngine.Random.Range();
        return AugmentType.None;
    }

    private float computeTotalProbability(){
        foreach(var item in LootpoolItems){
            totalRate += item.dropRate;
        }

        return totalRate;
    }
}
