using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHazard : MonoBehaviour
{
    [ReadOnly, SerializeReference] protected float playerSpeed;

    void LoadData() {
        playerSpeed = GetComponent<Movement>().GetSpeed();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //playerSpeed = GetComponent<Movement>().GetSpeed();
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
        GetComponent<Movement>().SetSpeed(playerSpeed);
    }
}
