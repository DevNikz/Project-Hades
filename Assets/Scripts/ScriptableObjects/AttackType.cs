using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "ProjectHades/Weapon/Sword", order = 1)]
[InlineEditor]
public class AttackType : ScriptableObject
{
    [Title("Properties")]
    public DamageType damageType;
    [Range(1f,1000f)] public float damage;
    [Range(1f,100f)] public float poise;
    [Range(5f, 1000f)] public float knocbackForce;
}
