using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [TitleGroup("Properties", "General Interactable Properties", Alignment = TitleAlignments.Centered)]
    [BoxGroup("Properties/Box1", ShowLabel = false)]
    [InfoBox("Popup Toggle (Disables Popup)"), OnValueChanged("UpdateToggle")]
    public bool toggle;
    private bool popupActivated = false;

    void Awake() {

    }

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
            popupActivated = true;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && popupActivated)
        {
            this.GetComponent<PopupController>().Disable();
            Destroy(this.gameObject);
        }
    }

    /*
    void OnTriggerExit() {
        this.GetComponent<PopupController>().Disable();
    }
    */
}
