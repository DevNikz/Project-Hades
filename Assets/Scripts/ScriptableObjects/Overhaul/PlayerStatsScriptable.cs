using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovement", menuName = "ProjectHades/Player/AttributesV2", order = 1)]
[InlineEditor]
public class PlayerStatsScriptable : ScriptableObject 
{
    [PropertySpace] [TitleGroup("Movement", "General Movement Properties", alignment: TitleAlignments.Centered)]
    [InfoBox("Player Move Speed (Default = 5)", InfoMessageType.None)]
    [Required] [Range(0.1f, 100f)] public float strafeSpeed = 5f;

    [PropertySpace] [InfoBox("Player Dash Speed / Force (Default = 25)", InfoMessageType.None)]
    [Required] [Range(0.1f, 100f)] public float dashForce = 25f;

    [PropertySpace] [InfoBox("Player Dash Cooldown (Default = 1.5)", InfoMessageType.None)]
    [Required] [Range(0.1f, 100f)] public float dashCooldown = 1.5f;

    [PropertySpace] [TitleGroup("Attributes", "General Player Attributes", alignment: TitleAlignments.Centered)]
    [InfoBox("Player Max HP (Default = 100)", InfoMessageType.None)]
    [Required] [Range(0f, 1000f)] public float maxHP = 100f;

    [PropertySpace] [InfoBox("Stun Resist (Default = 1)", InfoMessageType.None)]
    [Required] [Range(0.1f, 100f)] public float stunResist = 1f;

    [PropertySpace] [InfoBox("Damage Resist (Default = 1)", InfoMessageType.None)]
    [Required] [Range(0.1f, 100f)] public float damageResist = 1f;

    [PropertySpace] [TitleGroup("Extended Properties", "Additional Requirements / Misc. Stuffs", alignment: TitleAlignments.Centered)]
    [InfoBox("Player Turn Speed (Default = 720)")]
    [Required] [Range(0f, 1000f)] public float turnSpeed = 720f;

    [PropertySpace] [InfoBox("Player Friction (Default = 10)")]
    [Required] [Range(0f, 100f)] public float groundDrag = 10f;

    [PropertySpace] [InfoBox("Player Dash Duration (Default = 0.025)")]
    [Required] [Range(0f, 1f)] public float dashDuration = 0.025f;
}