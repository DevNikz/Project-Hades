using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] String NextLevel;
    [SerializeField] GameObject playerInputManager;

    [TitleGroup("Properties", "General Level Trigger Properties", TitleAlignments.Centered)]
    
    [HorizontalGroup("Properties/base")]
    [LabelWidth(120), BoxGroup("Properties/base/box1", ShowLabel = false)]
    [InfoBox("Toggle Trigger to enable Level TP")]
    public bool ToggleTrigger;

    [LabelWidth(120), BoxGroup("Properties/base/box2", ShowLabel = false)]
    [InfoBox("Current Enemy Counter")]
    public int enemyCounter;

    void Update() {
        enemyCounter = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemyCounter <= 0) ToggleTrigger = true; 
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            this.playerInputManager = GameObject.Find("PlayerInputManager");
            if(ToggleTrigger && playerInputManager != null) {

                playerInputManager.GetComponent<LevelRewardScript>().nextLevel = NextLevel;
                playerInputManager.GetComponent<LevelRewardScript>().Activate();

                //TransitionLevel();
                Destroy(this.GetComponent<Rigidbody>());
                Destroy(this.GetComponent<Collider>());
            }
        }
    }
}
