using System;
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
    [BoxGroup("Attributes/base/left/box1", showLabel: false)]
    [LabelWidth(100)][HideLabel][Title("Type", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)]
    public AugmentType augmentType;

    [VerticalGroup("Attributes/base/left")]
    [BoxGroup("Attributes/base/left/box1", showLabel: false)]
    [LabelWidth(100)][HideLabel][Title("PreReq", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)]
    public AugmentType preReqAugment;
    

    [VerticalGroup("Attributes/base/left")]
    [BoxGroup("Attributes/base/left/box1")]
    [PreviewField(125, Alignment = ObjectFieldAlignment.Center, FilterMode = FilterMode.Point)] [HideLabel] [Title("Icon", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)] 
    public Sprite augmentIcon;

    
    [VerticalGroup("Attributes/base/right")]
    [BoxGroup("Attributes/base/right/box2", showLabel: false)]
    [LabelWidth(100)][HideLabel] [Title("Description", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)]
    public string augmentDescription;

    [VerticalGroup("Attributes/base/right")]
    [BoxGroup("Attributes/base/right/box2", showLabel: false)]
    [LabelWidth(100)][HideLabel] [Title("Lore", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false, Bold = false)]
    public string augmentLore;

    public float augmentPower;
    public float augmentPower2;
    public float augmentPower3;

    [BoxGroup("Attributes/base/right/box3", showLabel: false)]
    [LabelWidth(50)]
    public bool IsActive {get; protected set;}

    [BoxGroup("Attributes/base/right/box3", showLabel: false)]
    [HorizontalGroup("Attributes/base/right/box3/split", 0.5f)]
    [Button("Call", ButtonSizes.Large)]
    public virtual void OnActivate() {
        this.IsActive = true;
    }

    [BoxGroup("Attributes/base/right/box3", showLabel: false)]
    [VerticalGroup("Attributes/base/right/box3/split/right")]
    [Button("Clear", ButtonSizes.Large)]
    public virtual void OnDeactivate() {
        this.IsActive = false;
    }

    public virtual void ActiveEffect(int count = 0){
        if(!this.IsActive) return;

    }
}
