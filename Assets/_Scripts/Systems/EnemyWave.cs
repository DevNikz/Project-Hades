using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveDetail", menuName = "ProjectHades/Waves", order = 1)]
[InlineEditor]

public class EnemyWave : ScriptableObject
{
    [Serializable] public class EnemyCountPair {
        [SerializeReference] public GameObject Enemy;
        [SerializeReference] public int Amount;
    }

    [PropertySpace]
    [TitleGroup("Enemies", "to Spawn", alignment: TitleAlignments.Centered)]
    [InfoBox("Enemies to Spawn", InfoMessageType.None)]
    [Required] public List<EnemyCountPair> EnemyList = new();
}
