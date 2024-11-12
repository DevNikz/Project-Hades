using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHazard : MonoBehaviour
{
    [ReadOnly, SerializeReference] protected float playerSpeed;

    void LoadData() {
        playerSpeed = GetComponent<Movement>().GetSpeed();
    }

    void Awake() {
        playerSpeed = GetComponent<Movement>().GetSpeed();
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        LoadData();
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("TriggerHazard")) {
            other.GetComponent<HazardController>().InitHazard(gameObject);
        }
    }

    void OnTriggerExit() {
        transform.Find("SpriteT").GetComponent<Animator>().speed = 1f;
        transform.Find("SpriteB").GetComponent<Animator>().speed = 1f;
        GetComponent<Movement>().SetSpeed(playerSpeed);
    }
}
