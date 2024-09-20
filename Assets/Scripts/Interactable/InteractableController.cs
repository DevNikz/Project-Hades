using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [TitleGroup("Properties", "General Interactable Properties", Alignment = TitleAlignments.Centered)]
    [BoxGroup("Properties/Box1", ShowLabel = false)]
    [InfoBox("Popup Toggle (Disables Popup)"), OnValueChanged("UpdateToggle")]
    public bool toggle;

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
