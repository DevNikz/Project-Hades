using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoScroll : MonoBehaviour
{
    public float speed = 100.0f;
    float textPosBegin = -1000.0f;
    float boundaryTextEnd = 1250.0f;

    RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI text;

    public bool hasStopped;

    private void OnEnable()
    {
        hasStopped = false;
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(AutoScrollText());
    }

    IEnumerator AutoScrollText()
    {
        while (rectTransform.localPosition.y < boundaryTextEnd)
        {
            rectTransform.Translate(Vector3.up * speed * Time.deltaTime);
            yield return null;
        }

        hasStopped = true;
    }

    private void Update()
    {
        if (hasStopped) GameObject.Find("CreditsCanvas").SetActive(false);
    }
}
