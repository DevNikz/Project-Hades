using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "ProjectHades/Weapon/Sword", order = 1)]
[InlineEditor]
public class AttackType : ScriptableObject
{
    [Title("Properties")]
    [SerializeField] public DamageType damageType;
    [SerializeField] [Range(1f,1000f)] public float damage;
    [SerializeField] [Range(1f,100f)] public float poise;
    [SerializeField] [Range(5f, 1000f)] public float knocbackForce;
}
