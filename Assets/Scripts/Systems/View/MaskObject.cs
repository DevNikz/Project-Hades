using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MaskObject : MonoBehaviour
{
    [PropertySpace, TitleGroup("Properties", "", TitleAlignments.Centered)] 
    [InfoBox("Assign Wall / Obstruction Layer and the Mask layer. Walls and Obstruction objects need to be assigned with the same Wall / Obstruction Layer")]
    [SerializeField] private LayerMask myLayerMask;

    [PropertySpace, TitleGroup("Reference", "", TitleAlignments.Centered)] 
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject target;
    [SerializeField] private string maskTagName;
    [SerializeField] private float fadedAlpha;
    [SerializeField] private float fadeTime;
    [SerializeField] private float FadeOffsetDistance;

    [SerializeField, HideInInspector] public List<GameObject> fadeableObjects;

    private float playerSqrdDistAtFloor;
    private Vector3 cameraAtFloor;

    void Start (){
        /* Calculates the camera's distance from the player at the floor level */
        cameraAtFloor = this.mainCamera.transform.position;
        cameraAtFloor.y = this.target.transform.position.y;

        this.playerSqrdDistAtFloor = Vector3.SqrMagnitude(cameraAtFloor - this.target.transform.position);
    }

    void Update() {
        // this.OldPeakSystem();
        this.NewPeakSystem();
    }



    void NewPeakSystem(){
        /* Calculates the camera's distance from the player at the floor level */
        cameraAtFloor = this.mainCamera.transform.position;
        cameraAtFloor.y = this.target.transform.position.y;

        /* Iterates through all objects that could be seen through */
        foreach(GameObject fadingObject in fadeableObjects){
            /* Skips null objects or objects missing the fadeablescript */
            if(fadingObject == null) continue;
            if(!fadingObject.TryGetComponent<FadeableObject>(out var fadeableRef)) continue;
                
            /* Calculates the ground distance of the object from the camera */
            Vector3 objectAtFloor = fadingObject.transform.position;
            objectAtFloor.y = cameraAtFloor.y;
            float objectDist = Vector3.SqrMagnitude(cameraAtFloor - objectAtFloor);
            
            /* Starts fadeout if the object is closer to the cam than the player w/ offset and fadein otherwise */
            if(objectDist <= playerSqrdDistAtFloor + (FadeOffsetDistance * FadeOffsetDistance))
                fadeableRef.StartTransition(true, fadeTime, fadedAlpha);
            else
                fadeableRef.StartTransition(false, fadeTime, fadedAlpha);
        }
    }

    void OldPeakSystem(){
        RaycastHit hit;

        if(Physics.Raycast(mainCamera.transform.position, 
        (target.transform.position - mainCamera.transform.position).normalized, 
        out hit, Mathf.Infinity, myLayerMask)) {
            //Debug.Log(hit.collider.name);

            if (hit.collider.gameObject.tag == "Mask") {
                target.transform.DOScale(0, 2);
            }
            else {
                //target.transform.DOScale(4 ,2);
                target.transform.DOScaleX(3, 2);
                target.transform.DOScaleY(1.5f, 2);
                target.transform.DOScaleZ(3, 2);
            }
        }
    }
}
