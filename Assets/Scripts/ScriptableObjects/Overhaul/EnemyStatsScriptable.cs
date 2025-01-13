using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ProjectHades/Enemy/AttributesV2", order = 1)]
[InlineEditor]
public class EnemyStatsScriptable : ScriptableObject 
{
    [PropertySpace] [TitleGroup("Attributes", "General Enemy Attributes", alignment: TitleAlignments.Centered)]
    [InfoBox("Enemy Type", InfoMessageType.None)]
    [Required] public EnemyType enemyType;

    [PropertySpace] [InfoBox("Enemy Max HP (Default = 100)", InfoMessageType.None)]
    [Required] [Range(0f, 10000f)] public float maxHP = 100f;

    [PropertySpace] [InfoBox("Enemy Max Poise (Default = 100)", InfoMessageType.None)]
    [Required] [Range(0f, 10000f)] public float maxPoise = 100f;

    [PropertySpace] [InfoBox("Stun Resist (Default = 1)", InfoMessageType.None)]
    [Required] [Range(0f, 100f)] public float stunResist = 1f;

    [PropertySpace] [InfoBox("Damage Resist (Default = 1)", InfoMessageType.None)]
    [Required] [Range(0f, 100f)] public float damageResist = 1f;
    
    [PropertySpace] [InfoBox("Attack Rate (Default = 0.5)", InfoMessageType.None)]
    [Required] [Range(0f, 100f)] public float attackRate = 0.5f;
    
    [PropertySpace] [InfoBox("Movement Speed (Default = 10)", InfoMessageType.None)]
    [Required] [Range(0f, 100f)] public float moveSpeed = 10f;
    
    [PropertySpace] [InfoBox("Stopping Distance (Default = 5)", InfoMessageType.None)]
    [Required] [Range(0f, 100f)] public float stoppingDistance = 5f;

    [PropertySpace] [TitleGroup("Timer Properties", "Enemy Timer Properties", alignment: TitleAlignments.Centered)]
    [InfoBox("Timer Delay", InfoMessageType.None)]
    [Required] [Range(0f, 100f)] public float timerDelay = 2f;

    [PropertySpace] [TitleGroup("Loot Properties", "Enemy Loot Drops!", alignment: TitleAlignments.Centered)]
    [AssetList(Path = "/Resources/Items")]
    [InfoBox("Assign Loot when Detained", InfoMessageType.None)]
    [Required] public GameObject[] lootDetained;

    [PropertySpace] [AssetList(Path = "/Resources/Items")]
    [InfoBox("Assign Loot when Killed", InfoMessageType.None)]
    [Required] public GameObject[] lootKilled;

    [PropertySpace, InfoBox("Scrap Count when killed", InfoMessageType.None)]
    [Required] [Range(1f, 10000f)] public int scrapCount = 1;

    [PropertySpace] [TitleGroup("References", "General References to Assets", alignment: TitleAlignments.Centered)]
    [AssetList(Path = "/Resources/Enemy/Animation")]
    [InfoBox("Assign Reference To Enemy Animation Clips", InfoMessageType.None)]
    [Required] public AnimationClip[] animationClips;

    [PropertySpace] [AssetList(Path = "/Resources/Enemy/Scripts")]
    [InfoBox("Assign Reference To Enemy AI Scripts", InfoMessageType.None)]
    [Required] public AnimationClip[] AIScripts;
    
}