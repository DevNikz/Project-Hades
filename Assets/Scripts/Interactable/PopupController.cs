using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

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

    [TitleGroup("References", "General Popup References", Alignment = TitleAlignments.Centered)]
    [BoxGroup("References/Box1", ShowLabel = false)]
    [InfoBox("Reference to Popup")]
    [SerializeReference] private GameObject popupRef;

    [BoxGroup("References/Box1", ShowLabel = false)]
    [InfoBox("Reference to Popup UI Text")]
    [SerializeReference] private TextMeshProUGUI textUI;

    [BoxGroup("References/Box1", ShowLabel = false)]
    [InfoBox("Reference to Interact Script")]
    [SerializeReference] private MonoBehaviour interactScript;

    void Update() {
        Interact();
    }

    void Interact() {
        if(toggle && Input.GetKeyDown("F")) {
            //Activate script or wtv
        }
        else if(toggle && Input.GetKeyDown("X")) {
            //Dismiss popup
            toggle = false;
        }
    }

    void previewTextUpdate() {
        if(this.previewText == "") {
            previewText = inputText.ToString();
        }
        else {
            previewText = "";
        }
    }

    public void Enable() {
        toggle = true;
        //Enable PopUp
    }

    public void Disable() {
        toggle = false;
    }
}