using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatStats", menuName = "ProjectHades/Player/PlayerCombatStats", order = 1)]
[InlineEditor]
public class PlayerCombatStats : ScriptableObject
{
    public float BaseMaxHealth;
    public float BaseMaxCharge;
    public float MoveSpeed;
    public float DashSpeed;
    public float DashTime;
    public float DashCooldown;
    public float BaseDamage;
    public float BasePoiseDamage;
    public float BaseCritRate;
    public float BaseKnockback;
}