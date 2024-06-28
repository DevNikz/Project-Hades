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

    void Awake() {
        movement = FindAnyObjectByType<Movement>();
        combat = FindAnyObjectByType<Combat>();

        speed = transform.Find("Panel").transform.Find("Speed").GetComponent<TextMeshProUGUI>();
        dash = transform.Find("Panel").transform.Find("Dash").GetComponent<TextMeshProUGUI>();
        lunge = transform.Find("Panel").transform.Find("Lunge").GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        speed.text = "Speed: " + movement.movement.currentSpeed;
        dash.text = "Dash: " + movement.movement.dashForce;
        lunge.text = "Lunge: " + combat.combat.quicklungeForce;
    }
}
