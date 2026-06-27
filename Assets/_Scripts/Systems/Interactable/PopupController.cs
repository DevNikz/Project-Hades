using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PopupController : MonoBehaviour 
{
    [TitleGroup("Properties", "General Popup Properties", Alignment = TitleAlignments.Centered)]

    [BoxGroup("Properties/Box1", ShowLabel = false)]
    [InfoBox("Current Status of Popup")]
    public bool toggle;

    [BoxGroup("Properties/Box1", ShowLabel = false)]
    [PropertySpace, InfoBox("Input Text (Text Asset / File) for Popup"), OnValueChanged("previewTextUpdate")]
    public TextAsset inputText;

    [BoxGroup("Properties/Box1", ShowLabel = false)]
    [PropertySpace, InfoBox("Preview Input Text"), TextArea(1, 10)]
    [ReadOnly] public string previewText = "";

    [Serializable]
    public class PopupEvent : UnityEvent {}

    [FormerlySerializedAs("Event")]
    [SerializeField] private PopupEvent popupEvent = new PopupEvent();

    [TitleGroup("References", "General Popup References", Alignment = TitleAlignments.Centered)]
    [BoxGroup("References/Box1", ShowLabel = false)]
    [InfoBox("Reference to Popup")]
    [SerializeReference] private GameObject popupRef;

    [BoxGroup("References/Box1", ShowLabel = false)]
    [InfoBox("Reference to Popup UI Text")]
    [SerializeReference] private TextMeshProUGUI textUI;

    void Update() {
        Interact();
        previewTextUpdate();
    }

    void Interact() {
        if(toggle && Input.GetKeyDown(KeyCode.F)) {
            //Activate script or wtv
            popupEvent.Invoke();
        }
        else if(toggle && Input.GetKeyDown(KeyCode.X)) {
            //Dismiss popup
            toggle = false;
            popupRef.SetActive(false);
        }
    }

    void previewTextUpdate() {
        if(inputText != null) {
            previewText = inputText.ToString();
            textUI.text = inputText.ToString();
        }
        else {
            previewText = "";
            textUI.text = "";
        }
    }

    public void Enable() {
        toggle = true;
        popupRef.SetActive(true);
        //Enable PopUp
    }

    public void Disable() {
        toggle = false;
        popupRef.SetActive(false);
    }
}