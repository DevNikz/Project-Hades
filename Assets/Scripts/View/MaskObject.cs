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

        /* Finds all objects that could be seen through */
        GameObject[] fadingObjects = GameObject.FindGameObjectsWithTag(maskTagName);
        foreach(GameObject fadingObject in fadingObjects){
            /* Skips the player check mask and objects wihtout renderers */
            if(fadingObject == target) continue;
            if(!fadingObject.TryGetComponent<Renderer>(out var renderer)) continue;
                
            /* Calculates the ground distance of the object from the camera */
            Vector3 objectAtFloor = fadingObject.transform.position;
            objectAtFloor.y = cameraAtFloor.y;
            float objectDist = Vector3.SqrMagnitude(cameraAtFloor - objectAtFloor);
            
            /* Selects Alpha between faded or not faded if the object is closer to the cam than the player w/ offset*/
            Color newColor = renderer.material.color;
            if(objectDist <= playerSqrdDistAtFloor + (FadeOffsetDistance * FadeOffsetDistance))
                newColor.a = fadedAlpha;
            else newColor.a = 1.0f;
            
            /* Starts the fader */
            StartCoroutine(TransitionFade(renderer, renderer.material.color, newColor));   
        }
    }

    IEnumerator TransitionFade(Renderer tar, Color startColor, Color endColor){
        float lerpStartTime = Time.time;
        float lerpProgress;
        bool lerping = true;

        while(lerping){
            yield return new WaitForEndOfFrame();

            lerpProgress = Time.time - lerpStartTime;

            if(tar != null){
                if(fadeTime == 0.0f){
                    tar.material.color = endColor;
                    lerping = false;
                } else {
                    tar.material.color = Color.Lerp(startColor, endColor, lerpProgress/fadeTime);
                }
                
            } else {
                lerping = false;
            }

            if(lerpProgress >= fadeTime){
                lerping = false;
            }
        }

        yield break;
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
