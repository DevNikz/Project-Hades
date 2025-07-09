using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    public float fps;
    private TextMeshProUGUI fpsText;

    void Awake() {
        fpsText = transform.Find("Time").transform.Find("FPS").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }

    void GetFPS() {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = fps + " FPS";
    }
}
