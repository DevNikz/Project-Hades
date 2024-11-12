using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "ProjectHades/General/Augment", order = 1)]
[InlineEditor]
public class AugmentScriptable : ScriptableObject 
{
    [TitleGroup("Attributes", "General Augment Attributes", TitleAlignments.Centered)]

    [HorizontalGroup("Attributes/base", Width = 135)]
    [VerticalGroup("Attributes/base/left")]
    [BoxGroup("Attributes/base/left/box1", LabelText = "Augment")]
    [LabelWidth(50)]
    [HideLabel] [Title("Name", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)]
    public string augmentName;

    [VerticalGroup("Attributes/base/left")]
    [BoxGroup("Attributes/base/left/box1")]
    [PreviewField(125, Alignment = ObjectFieldAlignment.Center, FilterMode = FilterMode.Point)] 
    [HideLabel] [Title("Icon", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)] 
    public Sprite augmentIcon;
    
    [VerticalGroup("Attributes/base/right")]
    [BoxGroup("Attributes/base/right/box2", showLabel: false)]
    [LabelWidth(100)]
    [HideLabel] [Title("Description", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)]
    public string augmentDescription;

    [VerticalGroup("Attributes/base/right")]
    [BoxGroup("Attributes/base/right/box2", showLabel: false)]
    [LabelWidth(100)]
    [HideLabel] [Title("Lore", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)]
    public string augmentLore;

    [BoxGroup("Attributes/base/right/box2", showLabel: false)]
    [LabelWidth(100)]
    [InfoBox("Augment Rarity (Default = Common)")]
    [Required] public Rarity augmentRarity = Rarity.Common;
    
    [BoxGroup("Attributes/base/right/box2", showLabel: false)]
    [LabelWidth(100)]
    [PropertySpace] [InfoBox("Augment Kind (Default = None)")]
    [Required] public Kind augmentKind = Kind.None;

    [BoxGroup("Attributes/base/right/box3", showLabel: false)]
    [LabelWidth(50)]
    public bool Toggle;

    [BoxGroup("Attributes/base/right/box3", showLabel: false)]
    [HorizontalGroup("Attributes/base/right/box3/split", 0.5f)]
    [Button("Call", ButtonSizes.Large)]
    private void TestCall() {
        this.Toggle = true;
    }

    [BoxGroup("Attributes/base/right/box3", showLabel: false)]
    [VerticalGroup("Attributes/base/right/box3/split/right")]
    [Button("Clear", ButtonSizes.Large)]
    private void TestClear() {
        this.Toggle = false;
    }
}
