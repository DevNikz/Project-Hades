using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueView _dialogueView;
    [SerializeField] public float TextCrawlTimePerCharacter;
    [SerializeField] private List<DialogueScriptable> _dialogueList = new();
    private Dictionary<string, DialogueScriptable> _dialogueDictionary = new();
    private DialogueScriptable _currentDialogue;
    private int _currentDialogueLineIndex = -1;

    public void StartDialoge(string DialogueTag)
    {
        if (!_dialogueView)
        {
            Debug.LogWarning("[WARN]: Dialogue View missing");
            return;
        }

        if (!_dialogueDictionary.ContainsKey(DialogueTag))
        {
            Debug.LogWarning($"[WARN]: Dialogue Tag {DialogueTag} not found");
            return;
        }

        if (_currentDialogue != null)
        {
            Debug.LogWarning($"[WARN]: Tried to start dialogue {DialogueTag} while another is running");
            return;
        }

        _currentDialogue = _dialogueDictionary[DialogueTag];
        _currentDialogueLineIndex = 0;

        _dialogueView.gameObject.SetActive(true);
    }

    public string GetNextDialogueLine()
    {
        if (_currentDialogue == null)
        {
            Debug.LogWarning("[WARN]: Tried to get dialogue line while no dialogue is running");
            CloseDialogue();
            return null;
        }

        if(_currentDialogueLineIndex >= _currentDialogue.DialogueLines.Count)
        {
            CloseDialogue();
            return null;
        }

        return _currentDialogue.DialogueLines[_currentDialogueLineIndex++];
    }

    private void CloseDialogue()
    {
        _currentDialogue = null;
        _currentDialogueLineIndex = -1;
        _dialogueView.gameObject.SetActive(false);
    }

    public void DialogueBoxClickCallback()
    {
        if (!_dialogueView)
        {
            Debug.LogWarning("[WARN]: Dialogue View missing");
            return;
        }
        _dialogueView.DialogueBoxClickCallback();
    }

    public static DialogueManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        LoadDialogueDict();
        CloseDialogue();
    }

    private void LoadDialogueDict()
    {
        _dialogueDictionary.Clear();

        foreach (var dialogue in _dialogueList)
        {
            _dialogueDictionary.Add(dialogue.DialogueTag, dialogue);
        }
    }
}
