using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StancePedestal : MonoBehaviour
{
    [SerializeField] private EStance _stance;
    void OnTriggerEnter(Collider other)
    {
        if (ItemManager.Instance == null) return;
        if (other.GetComponent<RevampPlayerController>() == null) return;

        ItemManager.Instance.HubSelectedStance = _stance;
        ItemManager.Instance.ClearAugmentsExceptStance(_stance);
    }
}
