using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatStats", menuName = "ProjectHades/Player/PlayerCombatStats", order = 1)][InlineEditor]
public class PlayerCombatStats : ScriptableObject
{
    public float BaseHealthDamage;
    public float BasePoiseDamage;
}