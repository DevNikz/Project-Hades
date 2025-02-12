using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerPreset", menuName = "ProjectHades/SpawnerWaves", order = 1)][InlineEditor]
public class SpawnerWaves : ScriptableObject
{
    [SerializeReference] public List<Waves> waves;
}
