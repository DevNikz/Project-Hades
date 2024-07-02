using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombat", menuName = "ProjectHades/Player/Combat", order = 1)]
[InlineEditor]
public class PlayerCombat : ScriptableObject
{
    [PropertySpace] [Title("Lunge")] 
    [Range(0.1f,100f)] public float lungeForce;
    [Range(0.1f,10f)] public float lungeForceMod;

    [PropertySpace] [Title("Quick Lunge")] 
    [Range(0.1f,100f)] public float quicklungeForce;
    [Range(0.1f,2f)] public float flicktime;
}
