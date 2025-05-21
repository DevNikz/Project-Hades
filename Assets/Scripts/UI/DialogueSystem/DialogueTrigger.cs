using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string _targetDialogueTag;
    [SerializeField] private bool _isOneshot;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            DialogueManager.Instance.StartDialoge(_targetDialogueTag);

            if (_isOneshot)
            {
                this.enabled = false;
            }
        }
    }
}
