using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptable", menuName = "ProjectHades/Dialogue", order = 1)]
public class DialogueScriptable : ScriptableObject
{
    public string DialogueTag;
    [TextArea(1, 5)]
    public List<string> DialogueLines;
}
