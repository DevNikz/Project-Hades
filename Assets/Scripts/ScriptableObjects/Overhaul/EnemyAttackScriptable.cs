using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "ProjectHades/Enemy/AttackV2", order = 2)]
[InlineEditor]
public class EnemyAttackScriptable : ScriptableObject
{
    [PropertySpace] [TitleGroup("Attributes", "General Attack Stats", alignment: TitleAlignments.Centered)]
    [InfoBox("Damage Value", InfoMessageType.None)]
    [Range(1f, 1000f)] public float healthDamage = 1f;

    [PropertySpace] [InfoBox("Stun Damage Value", InfoMessageType.None)]
    [Range(1f, 100f)] public float poiseDamage = 1f;

    [PropertySpace] [InfoBox("Knockback Force Value", InfoMessageType.None)]
    [Range(1f, 1000f)] public float knockbackForce = 1f;

    [PropertySpace] [TitleGroup("References", "References to Attack Animation Clips", alignment: TitleAlignments.Centered)]
    [AssetList(Path = "/Resources/Enemy/Animation")]
    [InfoBox("Assign Reference To Enemy Attack Animation Clips", InfoMessageType.None)]
    [Required] public AnimationClip[] attackAnimationClips;
}