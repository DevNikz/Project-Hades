using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    private Movement movement;
    private Combat combat;

    private TextMeshProUGUI speed;
    private TextMeshProUGUI dash;
    private TextMeshProUGUI lunge;
    private TextMeshProUGUI fixeddeltaTime;
    private TextMeshProUGUI state;

    void Awake() {
        movement = FindAnyObjectByType<Movement>();
        combat = FindAnyObjectByType<Combat>();

        speed = transform.Find("Player").transform.Find("Speed").GetComponent<TextMeshProUGUI>();
        dash = transform.Find("Player").transform.Find("Dash").GetComponent<TextMeshProUGUI>();
        lunge = transform.Find("Player").transform.Find("Lunge").GetComponent<TextMeshProUGUI>();

        fixeddeltaTime = transform.Find("Time").transform.Find("FixedDeltaTime").GetComponent<TextMeshProUGUI>();
        
        state = transform.Find("State").transform.Find("State").GetComponent<TextMeshProUGUI>();

    }

    void Update() {
        speed.text = "Speed: " + movement.currentSpeed;
        dash.text = "Dash: " + movement.movement.dashForce;
        lunge.text = "Lunge: " + combat.combat.quicklungeForce;
        fixeddeltaTime.text = "FixedDeltaTime: " + Time.fixedDeltaTime;
        state.text = "State: " + PlayerData.entityState;
    }
}
