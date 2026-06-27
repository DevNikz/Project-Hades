using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptable", menuName = "ProjectHades/Dialogue", order = 1)][InlineEditor]
public class DialogueScriptable : ScriptableObject
{
    public string DialogueTag;
    [TextArea(1, 5)]
    public List<string> DialogueLines;
}
