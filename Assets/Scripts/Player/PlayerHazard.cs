using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerHazard : MonoBehaviour
{
    [ReadOnly, SerializeReference] protected float playerSpeed;

    void OnEnable() {
        playerSpeed = GetComponent<Movement>().GetSpeed();
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
