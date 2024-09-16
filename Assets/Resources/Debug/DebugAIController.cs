using Sirenix.OdinInspector;
using UnityEngine;

public class DebugAIController : MonoBehaviour
{
    [TitleGroup("Properties", "General Debug Properties", Alignment = TitleAlignments.Centered)]

    public bool AIAggression;

    [Button("Toggle AI Aggression", ButtonSizes.Large)]
    private void ToggleAIAggression() {
        this.AIAggression = !this.AIAggression;
    }
}