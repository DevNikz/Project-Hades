using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttack", menuName = "ProjectHades/Player/AttackV2", order = 2)]
[InlineEditor]
public class PlayerAttackScriptable : ScriptableObject
{
    [PropertySpace] [TitleGroup("Properties", "Lunge", alignment: TitleAlignments.Centered)]
    [InfoBox("Force Applied to Attack (Default = 25)", InfoMessageType.None)]
    [Required] [Range(0.1f, 100f)] public float lungeForce = 25f;

    [PropertySpace] [InfoBox("Force Modifier applied to the Lunge (For Physics. Default = 1)", InfoMessageType.None)]
    [Required] [Range(0.1f, 10f)] public float lungeForceMod = 1f;

    [PropertySpace] [TitleGroup("Attributes", "General Attack Stats", alignment: TitleAlignments.Centered)]
    [InfoBox("Damage Value", InfoMessageType.None)]
    [Range(1f, 1000f)] public float healthDamage = 1f;

    [PropertySpace] [InfoBox("Stun Damage Value", InfoMessageType.None)]
    [Range(1f, 100f)] public float poiseDamage = 1f;

    [PropertySpace] [InfoBox("Knockback Force Value", InfoMessageType.None)]
    [Range(1f, 1000f)] public float knockbackForce = 1f;

    
    [PropertySpace] [TitleGroup("Optionals", "Quick Lunge", alignment: TitleAlignments.Centered)]
    public bool EnableOptionals;

    [ShowIfGroup("EnableOptionals")]
    [BoxGroup("EnableOptionals/Lunge", ShowLabel = false)]
    [InfoBox("Force Applied to 'Charged' Lunge (Default = 6)", InfoMessageType.None)]
    [Range(0.1f, 100f)] public float quickLungeForce = 6f;

    [BoxGroup("EnableOptionals/Lunge", ShowLabel = false)]
    [PropertySpace] [InfoBox("Timer for Charged Lunge (Default = 0.27)", InfoMessageType.None)]
    [Range(0.1f, 2f)] public float flicktime = 0.27f;

    [PropertySpace] [TitleGroup("References", "References to Attack Animation Clips", alignment: TitleAlignments.Centered)]
    [AssetList(Path = "/Resources/Player/Animation")]
    [InfoBox("Assign Reference To Player Attack Animation Clips", InfoMessageType.None)]
    [Required] public AnimationClip[] attackAnimationClips;
}


