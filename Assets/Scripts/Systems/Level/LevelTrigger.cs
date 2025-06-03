using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] String NextLevel;
    [SerializeField] GameObject playerInputManager;
    private EnemySpawner spawner;

    [TitleGroup("Properties", "General Level Trigger Properties", TitleAlignments.Centered)]
    
    [HorizontalGroup("Properties/base")]
    [LabelWidth(120), BoxGroup("Properties/base/box1", ShowLabel = false)]
    [InfoBox("Toggle Trigger to enable Level TP")]
    public bool TransitionWithEnemies = false;

    [LabelWidth(120), BoxGroup("Properties/base/box2", ShowLabel = false)]
    [InfoBox("Current Enemy Counter")]
    public int enemyCounter;
    public bool LoadsImmediatelyWithoutAugment = false;

    public static bool AtEndOfLevel { get; private set; } = false;

    void Awake() {
        if(playerInputManager == null) Debug.Log("Error. PlayerinputManager not detected");
        AtEndOfLevel = false;

        spawner = FindAnyObjectByType<EnemySpawner>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0 && !TransitionWithEnemies) return;

            AtEndOfLevel = true;

            this.playerInputManager = GameObject.Find("PlayerInputManager");
            if (!LoadsImmediatelyWithoutAugment && playerInputManager != null) {
                playerInputManager.GetComponent<LevelRewardScript>().Activate();
                
            }
            else
            {
                GameObject levelLoader = GameObject.Find("LevelLoader");
                if (levelLoader == null) {
                    Debug.LogWarning("[WARN]: LevelLoader not found");
                    return;
                }
                if(!levelLoader.TryGetComponent<LevelLoader>(out LevelLoader loader)){
                    Debug.LogWarning("[WARN]: LevelLoader component not found");
                    return;
                }
                if (SaveManager.Instance == null){
                    Debug.LogWarning("[WARN]: SaveManager null");
                    return;
                }
                
                loader.LoadLevel(SaveManager.Instance.GetNextLevel());
            }
            if(spawner != null)
                spawner.ClearSpawnPoints();

            Destroy(this.GetComponent<Rigidbody>());
            Destroy(this.GetComponent<Collider>());
            
        }
    }
}
