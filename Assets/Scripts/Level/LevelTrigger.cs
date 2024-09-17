using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] SceneAsset NextLevel;

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

    public void TransitionLevel() {
        SceneManager.LoadScene(NextLevel.name, LoadSceneMode.Single);
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            if(ToggleTrigger) {
                TransitionLevel();
                Destroy(this.GetComponent<Rigidbody>());
                Destroy(this.GetComponent<Collider>());
            }
        }
    }
}
