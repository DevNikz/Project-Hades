using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class InteractModule : MonoBehaviour
{

    // private void OnTriggerStay(Collider collider) {
    //     if(collider.gameObject.tag == "Interactable") {
    //         Debug.Log("Collided");

    //         //Set Reference
    //         // canvasReference.enabled = true;

    //         //Experimental Combat Debug Insert (lol)
    //             if(Input.GetKeyDown(KeyCode.E)) {
    //                 this.isInteractButtonPressed = true;
    //             }

    //             if(this.isInteractButtonPressed == true) {
    //                 color = new Color(0.25f, 1f, 0f, 1f);
    //                 meshRenderer.material.SetColor("_BaseColor", color);
    //             }
    //     }
    // }

    // private void OnTriggerExit(Collider collider) {
    //     if(collider.gameObject.tag == "Interactable") {
            
    //         this.isInteractButtonPressed = false;

    //         // //Set Reference
    //         // canvasReference.enabled = false;

    //         //Set Color
    //         Debug.Log("Exit");
    //         // color = new Color(0.541f, 0.541f, 0.541f, 1f);
    //         // meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();
    //         // meshRenderer.material.SetColor("_BaseColor", color);
    //     }
    // }

}
