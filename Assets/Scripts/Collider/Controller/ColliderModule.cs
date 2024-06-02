using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderModule : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] private PlayerCollider playerCollider;

    [Header("Interactable Properties")]

    //Types of Interactables
    [Header("Events")]
    [SerializeField] private InteractList interact;
    private MeshRenderer meshRenderer;
    private Color color;

    [Header("UI Properties")]
    [SerializeField] private PromptList prompt;

    private Parameters parameters;

    //Input Properties
    [SerializeField] public bool inputPress;
    [SerializeField] public bool inputHold;
    [SerializeField] public bool inputToggle;

    private bool toggleBool = false;

    public const string INPUT_PRESS = "INPUT_PRESS";
    public const string INPUT_HOLD = "INPUT_HOLD";
    public const string INPUT_TOGGLE = "INPUT_TOGGLE";

    private void Start() {
        playerCollider.collider.radius = playerCollider.radiusModifier;
    }

    private void OnTriggerEnter(Collider collider) {
        meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();
        //Detect Different Tags for Interactables

        //Event Interactable [Red]
        if(collider.gameObject.tag == "Event") {
            interact.eventList.Add(collider);
            color = new Color(1f, 0f, 0f, 1f);
            meshRenderer.material.SetColor("_BaseColor", color);
        }

        //Press Interactable [Blue]
        if(collider.gameObject.tag == "Press") {
            interact.pressList.Add(collider);
            color = new Color(0f, 0f, 1f, 1f);
            meshRenderer.material.SetColor("_BaseColor", color);
        }

        //Hold Interactable [Yellow]
        if(collider.gameObject.tag == "Hold") {
            interact.holdList.Add(collider);
            color = new Color(1f, 0.88f, 0f, 1f);
            meshRenderer.material.SetColor("_BaseColor", color);
        }

        //Toggle Interactable [Green]
        if(collider.gameObject.tag == "Toggle") {
            interact.toggleList.Add(collider);
            color = new Color(0f, 1f, 0f, 1f);
            meshRenderer.material.SetColor("_BaseColor", color);
        }

        if(collider.gameObject.tag == "UI") {
            prompt.promptList.Add(collider);
            
            parameters = new Parameters();
            parameters.PutExtra(PromptManager.PROMPT_NAME, collider.gameObject.name);

            EventBroadcaster.Instance.PostEvent(EventNames.Prompt.PROMPT_NAMES_ADD, parameters);
        }
    }

    private void OnTriggerStay(Collider collider) {
        if(collider.gameObject.tag == "Press") {
            EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.InputPress);
            if(this.inputPress) {
                Debug.Log("Pressed");
                parameters = new Parameters();
                parameters.PutExtra(ObjectManager.INTERACT_NAME, collider.gameObject.name);
                EventBroadcaster.Instance.PostEvent(EventNames.Active.INTERACT_ENABLE, parameters);
            }

            else {
                parameters = new Parameters();
                parameters.PutExtra(ObjectManager.INTERACT_NAME, collider.gameObject.name);
                EventBroadcaster.Instance.PostEvent(EventNames.Active.INTERACT_DISABLE, parameters);
            }
        }

        if(collider.gameObject.tag == "Hold") {
            EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_HOLD, this.InputHold);
            if(this.inputHold) {
                Debug.Log("Holding Input.");
            }
            else {
                Debug.Log("Released Input");
            }
        }

        if(collider.gameObject.tag == "Toggle") {
            EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_TOGGLE, this.InputToggle);
            if(this.inputToggle) {
                if(this.toggleBool == false) {
                    Debug.Log("Toggle On");
                    this.toggleBool = true;
                }
                else {
                    Debug.Log("Toggle Off");
                    this.toggleBool = false;
                }

            }
        }
    }

    private void InputPress(Parameters parameters) {
        this.inputPress = parameters.GetBoolExtra(INPUT_PRESS, false);
    }

    private void InputHold(Parameters parameters) {
        this.inputHold = parameters.GetBoolExtra(INPUT_HOLD, false);
    }

    private void InputToggle(Parameters parameters) {
        this.inputToggle = parameters.GetBoolExtra(INPUT_TOGGLE, false);
    }

    private void OnTriggerExit(Collider collider) {
        

        //Event Interactable [Red]
        if(collider.gameObject.tag == "Event") {
            interact.eventList.Remove(collider);
        }

        //Press Interactable [Blue]
        if(collider.gameObject.tag == "Press") {
            EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);

            parameters = new Parameters();
            parameters.PutExtra(ObjectManager.INTERACT_NAME, collider.gameObject.name);

            EventBroadcaster.Instance.PostEvent(EventNames.Active.INTERACT_EXIT, parameters);
            interact.pressList.Remove(collider);
        }

        //Hold Interactable [Yellow]
        if(collider.gameObject.tag == "Hold") {
            EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_HOLD);

            parameters = new Parameters();
            parameters.PutExtra(ObjectManager.INTERACT_NAME, collider.gameObject.name);

            EventBroadcaster.Instance.PostEvent(EventNames.Active.INTERACT_EXIT, parameters);
            interact.holdList.Remove(collider);
        }

        //Toggle Interactable [Green]
        if(collider.gameObject.tag == "Toggle") {
            EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_TOGGLE);

            parameters = new Parameters();
            parameters.PutExtra(ObjectManager.INTERACT_NAME, collider.gameObject.name);

            EventBroadcaster.Instance.PostEvent(EventNames.Active.INTERACT_EXIT, parameters);
            interact.toggleList.Remove(collider);
        }

        if(collider.gameObject.tag == "UI") {
            prompt.promptList.Remove(collider);

            parameters = new Parameters();
            parameters.PutExtra(PromptManager.PROMPT_NAME, collider.gameObject.name);

            EventBroadcaster.Instance.PostEvent(EventNames.Prompt.PROMPT_NAMES_DELETE, parameters);
        }
    }

}
