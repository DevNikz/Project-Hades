using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Wave Set", menuName = "ProjectHades/EnemyWaveSet")]
public class EnemyWaveSet : ScriptableObject
{

    [SerializeReference] public List<EnemyWave> EnemyWaves = new();
}
