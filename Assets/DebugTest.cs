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
    private TextMeshProUGUI deltaTime;
    private TextMeshProUGUI fixeddeltaTime;

    void Awake() {
        movement = FindAnyObjectByType<Movement>();
        combat = FindAnyObjectByType<Combat>();

        speed = transform.Find("Player").transform.Find("Speed").GetComponent<TextMeshProUGUI>();
        dash = transform.Find("Player").transform.Find("Dash").GetComponent<TextMeshProUGUI>();
        lunge = transform.Find("Player").transform.Find("Lunge").GetComponent<TextMeshProUGUI>();

        deltaTime = transform.Find("Time").transform.Find("DeltaTime").GetComponent<TextMeshProUGUI>();
        fixeddeltaTime = transform.Find("Time").transform.Find("FixedDeltaTime").GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        speed.text = "Speed: " + movement.currentSpeed;
        dash.text = "Dash: " + movement.movement.dashForce;
        lunge.text = "Lunge: " + combat.combat.quicklungeForce;
        deltaTime.text = "DeltaTime: " + Time.deltaTime;
        fixeddeltaTime.text = "FixedDeltaTime: " + Time.fixedDeltaTime;
    }
}
