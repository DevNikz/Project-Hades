using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    private GameObject healthUI;
    private GameObject poiseUI;
    private GameObject baseEnemy;

    void Start() {
        healthUI = transform.Find("Health").gameObject;
        poiseUI = transform.Find("Poise").gameObject;
        baseEnemy = transform.Find("Base").gameObject;
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI() {
        healthUI.GetComponent<RectTransform>().position = new Vector3(baseEnemy.transform.position.x, 0.7f, baseEnemy.transform.position.z);
        poiseUI.GetComponent<RectTransform>().position = new Vector3(baseEnemy.transform.position.x, 1.1f, baseEnemy.transform.position.z);

        healthUI.GetComponent<RectTransform>().localRotation = Quaternion.Euler(45f,45f,0f);
        poiseUI.GetComponent<RectTransform>().localRotation = Quaternion.Euler(45f,45f,0f);
    }
}
