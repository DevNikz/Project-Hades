using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerElementStyle", menuName = "ProjectHades/Player/ElementStyle", order = 3)]
[InlineEditor]
public class PlayerElementScriptable : ScriptableObject 
{
    [PropertySpace] [TitleGroup("References", "References to Animations and Styles", alignment: TitleAlignments.Centered)]
    [AssetList(Path = "/Resources/Player/Animation")]
    [InfoBox("Assign Reference to Player Animations (Animation Clips)", InfoMessageType.None)]
    [Required] public AnimationClip[] animationClips;

    [PropertySpace] [AssetList(Path = "/Resources/Player/Styles")]
    [InfoBox("Assign Reference to Player Attack Styles (ScriptableObjects)", InfoMessageType.None)]
    [Required] public ScriptableObject[] attackStyles;

    [Range(0, 100)]
    [InfoBox("Earth Stance")]
    public int staggerDamage;

    [Range(0, 100)]
    public int staggerDamageCharged;

    [Range(0, 100)]
    [InfoBox("Water Stance")]
    public int attackRange;

    [Range(0, 100)]
    public int attackRangeCharged;


    [Range(0, 100)]
    [InfoBox("Wind Stance")]
    public int attackSpeed;

    [Range(0, 100)]
    public int attackSpeedCharged;

    [Range(0, 100)]
    [InfoBox("Fire Stance")]
    public int attackDamage;

    [Range(0, 100)]
    public int attackDamageCharged;
}