using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderModule : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] private PlayerCollider playerCollider;

    [Header("Interactable Properties")]
    [SerializeField] private InteractList interact;
    private MeshRenderer meshRenderer;
    private Color color;
    private bool isInteractButtonPressed = false;

    [Header("UI Properties")]
    [SerializeField] private PromptList prompt;

    private Parameters parameters;

    private void Start() {
        playerCollider.collider.radius = playerCollider.radiusModifier;
    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag == "Interactable") {
            interact.interactList.Add(collider);

            meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();

            //Debug
            color = new Color(1f, 1f, 1f, 1f);
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
        if(collider.gameObject.tag == "Interactable") {

            //Experimental Combat Debug Insert (lol)
            if(Input.GetKeyDown(KeyCode.E)) {
                parameters = new Parameters();
                parameters.PutExtra(InteractManager.INTERACT_NAME, collider.gameObject.name);

                EventBroadcaster.Instance.PostEvent(EventNames.Active.INTERACT_ENABLE, parameters);
            }
        }
    }

    private void OnTriggerExit(Collider collider) {
        if(collider.gameObject.tag == "Interactable") {

            parameters = new Parameters();
            parameters.PutExtra(InteractManager.INTERACT_NAME, collider.gameObject.name);

            EventBroadcaster.Instance.PostEvent(EventNames.Active.INTERACT_DISABLE, parameters);

            //Remove From List
            interact.interactList.Remove(collider);
        }

        if(collider.gameObject.tag == "UI") {
            prompt.promptList.Remove(collider);

            parameters = new Parameters();
            parameters.PutExtra(PromptManager.PROMPT_NAME, collider.gameObject.name);

            EventBroadcaster.Instance.PostEvent(EventNames.Prompt.PROMPT_NAMES_DELETE, parameters);
        }
    }

}
