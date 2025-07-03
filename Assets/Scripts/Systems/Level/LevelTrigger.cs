using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] String NextLevel;
    [SerializeField] GameObject playerInputManager;

    [TitleGroup("Properties", "General Level Trigger Properties", TitleAlignments.Centered)]

    [HorizontalGroup("Properties/base")]
    [LabelWidth(120), BoxGroup("Properties/base/box1", ShowLabel = false)]
    [InfoBox("Toggle Trigger to enable Level TP")]
    public bool TransitionWithEnemies = false;

    [LabelWidth(120), BoxGroup("Properties/base/box2", ShowLabel = false)]
    [InfoBox("Current Enemy Counter")]
    public int enemyCounter;
    public bool LoadsImmediatelyWithoutAugment = false;

    [SerializeField] private string _endLevelDialogueTag;
    [SerializeField] private bool _dialogueTagIsDatabase;

    public static bool AtEndOfLevel { get; private set; } = false;

    void Awake()
    {
        // if (playerInputManager == null) Debug.LogWarning("Error. PlayerinputManager not detected");
        AtEndOfLevel = false;
        _procedGudiingArrow = false;
    }

    private bool _procedGudiingArrow = false;
    void Update()
    {
        if (!_procedGudiingArrow && EnemySpawner.Instance != null)
        {
            if (EnemySpawner.Instance.AreWavesOver && GuidingArrow.Instance != null)
            {
                _procedGudiingArrow = true;
                GuidingArrow.Instance.Enable(gameObject.transform);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0 && !TransitionWithEnemies) return;

            if (!TryStartDialogue()) FinishLevel();
        }
    }

    private bool TryStartDialogue()
    {
        if (_endLevelDialogueTag.IsNullOrWhitespace()) return false;
        if (DialogueManager.Instance == null) return false;

        if(_dialogueTagIsDatabase)
            DialogueManager.Instance.StartRandomDialogueFromDatabase(_endLevelDialogueTag, FinishLevel);
        else
            DialogueManager.Instance.StartDialogue(_endLevelDialogueTag, FinishLevel);

        return true;
    }

    public void FinishLevel()
    {
        AtEndOfLevel = true;
        if (EnemySpawner.Instance != null)
            EnemySpawner.Instance.Deactivate();

        this.playerInputManager = GameObject.Find("PlayerInputManager");
        if (!LoadsImmediatelyWithoutAugment && playerInputManager != null)
        {
            playerInputManager.GetComponent<LevelRewardScript>().Activate(false);

        }
        else
        {
            GameObject levelLoader = GameObject.Find("LevelLoader");
            if (levelLoader == null)
            {
                Debug.LogWarning("[WARN]: LevelLoader not found");
                return;
            }
            if (!levelLoader.TryGetComponent<LevelLoader>(out LevelLoader loader))
            {
                Debug.LogWarning("[WARN]: LevelLoader component not found");
                return;
            }
            
            if (SaveManager.Instance == null)
            {
                Debug.LogWarning("[WARN]: SaveManager null");
                return;
            }

            Debug.Log("Level Over");
            SaveManager.Instance.AddDepth();
            loader.LoadLevel(SaveManager.Instance.GetNextLevel());
        }

        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this.GetComponent<Collider>());
    }
}
