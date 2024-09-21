using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractableController : MonoBehaviour
{
    [TitleGroup("Properties", "General Interactable Properties", Alignment = TitleAlignments.Centered)]
    [BoxGroup("Properties/Box1", ShowLabel = false)]
    [InfoBox("Popup Toggle (Disables Popup)"), OnValueChanged("UpdateToggle")]
    public bool toggle;

    [BoxGroup("Properties/Box1", ShowLabel = false)]
    [Button("Interact", ButtonSizes.Large)]
    void InteractScript() {
        //Should invoke script
        interact.Invoke();
    }

    [Serializable]
    public class Interact : UnityEvent {}

    [FormerlySerializedAs("Interact")]
    [SerializeField] private Interact interact = new Interact();



    void UpdateToggle() {
        if(!toggle) {
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<PopupController>().enabled = false;
        }
        else {
            this.GetComponent<Collider>().enabled = true;
            this.GetComponent<PopupController>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            this.GetComponent<PopupController>().Enable();
        }
    }

    void OnTriggerExit() {
        this.GetComponent<PopupController>().Disable();
    }
}
