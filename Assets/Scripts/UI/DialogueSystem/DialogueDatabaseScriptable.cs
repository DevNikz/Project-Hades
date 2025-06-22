using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDatabaseScriptable", menuName = "ProjectHades/DialogueDatabase", order = 1)][InlineEditor]
public class DialogueDatabaseScriptable : ScriptableObject
{
    public string DatabaseTag;
    [SerializeReference]public List<DialogueScriptable> Dialogues = new();
    public DialogueScriptable GetRandomDialogue()
    {
        if (Dialogues.Count <= 0) return null;

        return Dialogues[Random.Range(0, Dialogues.Count)];
    }
}
