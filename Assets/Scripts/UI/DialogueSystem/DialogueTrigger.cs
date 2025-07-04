using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string _targetDialogueTag;
    [SerializeField] private bool _isOneshot;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<RevampPlayerController>(out var player))
        {
            DialogueManager.Instance.StartDialogue(_targetDialogueTag);

            if (_isOneshot)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
