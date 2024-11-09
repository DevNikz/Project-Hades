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

    private float playerSqrdDist;

    void Start (){
        this.playerSqrdDist = Vector3.SqrMagnitude(this.mainCamera.transform.position - this.target.transform.position);
    }

    void Update() {
        // this.OldPeakSystem();
        this.NewPeakSystem();
    }

    void NewPeakSystem(){
        GameObject[] fadingObjects = GameObject.FindGameObjectsWithTag(maskTagName);
        Debug.Log("Found Object Size: " + fadingObjects.Length);
        foreach(GameObject fadingObject in fadingObjects){
            Debug.Log("Found Fading Target: " + fadingObject.name);
            if(fadingObject == target) continue;

            Renderer renderer = fadingObject.GetComponent<Renderer>();
            
            if(renderer == null){
                Debug.Log("Found Renderer for " + fadingObject.name + ": False");
                continue;
            } 
                
            Debug.Log("Found Renderer for " + fadingObject.name + ": True");

            float objectDist = Vector3.SqrMagnitude(this.mainCamera.transform.position - fadingObject.transform.position);
            // Debug.Log(fadingObject.name + " is " + objectDist + "; Player is " + playerSqrdDist);

            Color newColor = renderer.material.color;
            if(objectDist <= playerSqrdDist)
                newColor.a = fadedAlpha;
            else 
                newColor.a = 1.0f;
            StartCoroutine(TransitionFade(renderer, renderer.material.color, newColor));
            
        }

        // RaycastHit hit;

        // if(Physics.Raycast(mainCamera.transform.position, 
        // (target.transform.position - mainCamera.transform.position).normalized, 
        // out hit, Mathf.Infinity, myLayerMask)) {
        //     //Debug.Log(hit.collider.name);

        //     if (hit.collider.gameObject.tag == "Mask") {
        //         if(hit.collider.gameObject.TryGetComponent<Renderer>(out var renderer)){
        //             if(renderer == null) return;
        //             Color fadedColor = renderer.material.color;
        //             fadedColor.a = fadedAlpha;
        //             Debug.Log("FirstColor: " + renderer.material.color);
        //             Debug.Log("TargetColor: " + fadedColor);
        //             StartCoroutine(TransitionFade(renderer, renderer.material.color, fadedColor));
        //             fadeTarget = hit.collider.gameObject;
        //         }
        //     } else if (hit.collider.gameObject == target) {
        //         if(fadeTarget.TryGetComponent<Renderer>(out var renderer)){
        //             if(renderer == null) return;
        //             Color solidColor = renderer.material.color;
        //             solidColor.a = 1.0f;
        //             Debug.Log("FirstColor: " + renderer.material.color);
        //             Debug.Log("TargetColor: " + solidColor);
        //             StartCoroutine(TransitionFade(renderer, renderer.material.color, solidColor));
        //         }
        //     }
            
        // }
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
