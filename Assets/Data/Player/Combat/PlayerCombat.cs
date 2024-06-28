using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombat", menuName = "ProjectHades/Player/Combat", order = 1)]
[InlineEditor]
public class PlayerCombat : ScriptableObject
{
    [Space] [Title("Lunge")] 
    [SerializeField] [Range(10f,50f)] public float lungeForce = 25f;  

    [Space] [Title("Quick Lunge")] 
    [SerializeField] [Range(0.1f,20f)] public float quicklungeForce = 15f;
    [SerializeField] [Range(0.1f,2f)] public float flicktime = 0.27f;
}
