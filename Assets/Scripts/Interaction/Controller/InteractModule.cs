using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class InteractModule : MonoBehaviour
{

    //InteractionCollider Properties
    [Header("Interaction Module")]

    [SerializeField] public InteractCollider intCollider;

    [ReadOnly] private MeshRenderer meshRenderer;

    private Color color;

    //UI Interaction Properties
    [Header("UI Interaction Module")]

    [Tooltip("Set Canvas Group Reference")]
    [ReadOnly] public Canvas canvasReference;

    [Tooltip("Is Interact Prompt Active? (Defualt = false)")]
    [SerializeField] public bool isPromptActive = false;
    
    [Tooltip("Is Button Pressed For Promot? (Default = false)")]
    [SerializeField] public bool isInteractButtonPressed = false;

    private void Start() {
        canvasReference.enabled = false;
    }

    private void Update() {
        StateRadius();
        CheckPromptActive();
    }

    private void StateRadius() {
        intCollider.collider.radius = intCollider.radiusModifier;
    }

    private void CheckPromptActive() {
        if(canvasReference.enabled == true) this.isPromptActive = true;
        else this.isPromptActive = false;
    }


    private void OnTriggerStay(Collider collider) {
        if(collider.gameObject.tag == "Interactable") {
            Debug.Log("Collided");

            //Set Reference
            canvasReference.enabled = true;
            meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();

            //Set Color
            color = new Color(1f, 1f, 1f, 1f);
            meshRenderer.material.SetColor("_BaseColor", color);

            //Experimental Combat Debug Insert (lol)
                if(Input.GetKeyDown(KeyCode.E)) {
                    this.isInteractButtonPressed = true;
                }

                if(this.isInteractButtonPressed == true) {
                    color = new Color(0.25f, 1f, 0f, 1f);
                    meshRenderer.material.SetColor("_BaseColor", color);
                }
        }
    }

    private void OnTriggerExit(Collider collider) {
        if(collider.gameObject.tag == "Interactable") {
            
            this.isInteractButtonPressed = false;

            //Set Reference
            canvasReference.enabled = false;

            //Set Color
            Debug.Log("Exit");
            color = new Color(0.541f, 0.541f, 0.541f, 1f);
            meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material.SetColor("_BaseColor", color);
        }
    }

}
