using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] String NextLevel;
    [SerializeField] GameObject playerInputManager;
    private Spawner spawner;

    [TitleGroup("Properties", "General Level Trigger Properties", TitleAlignments.Centered)]
    
    [HorizontalGroup("Properties/base")]
    [LabelWidth(120), BoxGroup("Properties/base/box1", ShowLabel = false)]
    [InfoBox("Toggle Trigger to enable Level TP")]
    public bool ToggleTrigger;

    [LabelWidth(120), BoxGroup("Properties/base/box2", ShowLabel = false)]
    [InfoBox("Current Enemy Counter")]
    public int enemyCounter;

    private static bool hudCheck;

    public static bool HudCheck {
        get { return hudCheck; }
        set { hudCheck = value; }
    }

    void Awake() {
        if(playerInputManager == null) Debug.Log("Error. PlayerinputManager not detected");
        hudCheck = false;

        spawner = FindAnyObjectByType<Spawner>();
    }


    void Update() {
        if (spawner == null)
        {
            enemyCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCounter <= 0) ToggleTrigger = true;
        }
        else if (spawner.FinalWave)
        {
            enemyCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCounter <= 0) ToggleTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            this.playerInputManager = GameObject.Find("PlayerInputManager");
            if(ToggleTrigger && playerInputManager != null) {

                hudCheck = true;

                //if(NextLevel == "Win Screen") Destroy(GameObject.Find("LevelSystems"));

                playerInputManager.GetComponent<LevelRewardScript>().nextLevel = NextLevel;
                playerInputManager.GetComponent<LevelRewardScript>().Activate();

                //TransitionLevel();
                Destroy(this.GetComponent<Rigidbody>());
                Destroy(this.GetComponent<Collider>());
            }
        }
    }
}
