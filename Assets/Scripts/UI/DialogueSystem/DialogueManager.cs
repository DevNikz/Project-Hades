using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueView _dialogueView;
    [SerializeField] public float TextCrawlTimePerCharacter;
    [SerializeField] private float _interactableSpamBufferTime;
    [SerializeReference] private DialogueDatabaseScriptable _coreDatabase;
    [SerializeReference] private List<DialogueDatabaseScriptable> _dialogueDatabases = new();
    private Dictionary<string, DialogueScriptable> _dialogueDictionary = new();
    private Dictionary<string, DialogueDatabaseScriptable> _dialogueDatabaseDictionary = new();
    private DialogueScriptable _currentDialogue;

    public delegate void EndDialogueCallback();
    private EndDialogueCallback _endDialogueCallback = null;
    private float _dialogueInteractableCountdown = 0f;

    private int _currentDialogueLineIndex = -1;
    public void StartDialogue(string DialogueTag, EndDialogueCallback callback = null)
    {
        if (!_dialogueView)
        {
            Debug.LogWarning("[WARN]: Dialogue View missing");
            callback?.Invoke();
            return;
        }

        if (!_dialogueDictionary.ContainsKey(DialogueTag))
        {
            Debug.LogWarning($"[WARN]: Dialogue Tag {DialogueTag} not found");
            callback?.Invoke();
            return;
        }

        if (_currentDialogue != null)
        {
            Debug.LogWarning($"[WARN]: Tried to start dialogue {DialogueTag} while another is running");
            callback?.Invoke();
            return;
        }

        _dialogueInteractableCountdown = _interactableSpamBufferTime;

        _currentDialogue = _dialogueDictionary[DialogueTag];
        _currentDialogueLineIndex = 0;

        _dialogueView.gameObject.SetActive(true);
        _dialogueView.DialogueBoxClickCallback();
        Time.timeScale = 0;
        _endDialogueCallback = callback;

    }
    public void StartRandomDialogueFromDatabase(string DatabaseTag, EndDialogueCallback callback = null)
    {
        if (!_dialogueView)
        {
            Debug.LogWarning("[WARN]: Dialogue View missing");
            callback?.Invoke();
            return;
        }

        if (!_dialogueDatabaseDictionary.ContainsKey(DatabaseTag))
        {
            Debug.LogWarning($"[WARN]: Dialogue Database Tag {DatabaseTag} not found");
            callback?.Invoke();
            return;
        }

        if (_currentDialogue != null)
        {
            Debug.LogWarning($"[WARN]: Tried to start random dialogue from {DatabaseTag} while another is running");
            callback?.Invoke();
            return;
        }

        _dialogueInteractableCountdown = _interactableSpamBufferTime;

        _currentDialogue = _dialogueDatabaseDictionary[DatabaseTag].GetRandomDialogue();
        _currentDialogueLineIndex = 0;

        _dialogueView.gameObject.SetActive(true);
        _dialogueView.DialogueBoxClickCallback();
        Time.timeScale = 0;

        _endDialogueCallback = callback;
    }

    public string GetNextDialogueLine()
    {
        if (_currentDialogue == null)
        {
            Debug.LogWarning("[WARN]: Tried to get dialogue line while no dialogue is running");
            CloseDialogue();
            return null;
        }

        if (_currentDialogueLineIndex >= _currentDialogue.DialogueLines.Count)
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
        Time.timeScale = 1;

        _endDialogueCallback?.Invoke();
        _endDialogueCallback = null;
    }

    public void DialogueBoxClickCallback()
    {
        if (_dialogueInteractableCountdown > 0)
            return;

        if (!_dialogueView)
        {
            Debug.LogWarning("[WARN]: Dialogue View missing");
            return;
        }
        _dialogueInteractableCountdown = _interactableSpamBufferTime;
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
        _dialogueDatabaseDictionary.Clear();

        foreach (var dialogue in _coreDatabase.Dialogues)
        {
            _dialogueDictionary.Add(dialogue.DialogueTag, dialogue);
        }

        foreach (var database in _dialogueDatabases)
        {
            _dialogueDatabaseDictionary.Add(database.DatabaseTag, database);
        }
    }

    void Update()
    {
        if (_dialogueInteractableCountdown > 0)
            _dialogueInteractableCountdown -= Time.unscaledDeltaTime;   
    }
}
