using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Augment", menuName = "ProjectHades/General/VitalityGear")]
[InlineEditor]
public class VitalityGearScriptable : AugmentScriptable 
{
    [BoxGroup("Attributes/base/right/box3", showLabel: false)]
    [HorizontalGroup("Attributes/base/right/box3/split", 0.5f)]
    [Button("Call", ButtonSizes.Large)]
    public override void OnActivate() {
        this.IsActive = true;
        // TODO : Update the player stats accordingly
        // Debug.LogWarning("[TODO]: Vitality Gear needs to update player health stats");
    }

    [BoxGroup("Attributes/base/right/box3", showLabel: false)]
    [VerticalGroup("Attributes/base/right/box3/split/right")]
    [Button("Clear", ButtonSizes.Large)]
    public override void OnDeactivate() {
        this.IsActive = false;
        // TODO : Update the player stats accordingly
        // Debug.LogWarning("[TODO]: Vitality Gear needs to update player health stats on deactivate");
    }
}
