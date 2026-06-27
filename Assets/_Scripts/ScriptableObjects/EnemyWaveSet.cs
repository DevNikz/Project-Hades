using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Wave Set", menuName = "ProjectHades/EnemyWaveSet")][InlineEditor]
public class EnemyWaveSet : ScriptableObject
{
    [SerializeReference] public List<EnemyWave> EnemyWaves = new();
}
