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
}