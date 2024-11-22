using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveDetail", menuName = "ProjectHades/Waves", order = 1)]
[InlineEditor]

public class Waves : ScriptableObject
{
    [PropertySpace]
    [TitleGroup("Enemies", "to Spawn", alignment: TitleAlignments.Centered)]
    [InfoBox("Enemies to Spawn", InfoMessageType.None)]
    [Required] public GameObject[] Enemies;

    [PropertySpace]
    [InfoBox("Amount to Spawn", InfoMessageType.None)]
    [Required] public int[] Amount;
}
