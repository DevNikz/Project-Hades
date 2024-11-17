using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetainIconScript : MonoBehaviour
{
    bool playerSeen;

    private Image icon = null;
    [SerializeField] private GameObject ui = null;

    // Start is called before the first frame update
    void Start()
    {
        playerSeen = false;

        icon = ui.transform.Find("DetainIcon").gameObject.GetComponent<Image>();

        if (icon == null) Debug.Log("not found");

        icon.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player Seen: " + playerSeen);
            if(!playerSeen) icon.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        icon.enabled = false;
    }

    private void OnEnable()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.Combat.PLAYER_SEEN, SetPlayerSeen);
    }

    private void OnDisable()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Combat.PLAYER_SEEN);
    }

    void SetPlayerSeen(Parameters param)
    {
        playerSeen = param.GetBoolExtra("HIDDEN", false);
    }
}
