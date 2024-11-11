using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeableObject : MonoBehaviour
{
    [SerializeField] private GameObject solidModel;
    [SerializeField] private GameObject transparentModel;
    [SerializeField, HideInInspector] private Renderer[] renderers;

    bool lerping = false;
    bool fadedout = false;

    void Start(){
        if(this.solidModel == null || this.transparentModel == null){
            Destroy(this);
        }

        renderers = this.transparentModel.GetComponentsInChildren<Renderer>(true);
        if(renderers.Length == 0){
            Destroy(this);
        }

        this.transparentModel.SetActive(false);
        this.solidModel.SetActive(true);
    }

    public void StartTransition(bool isFadeout, float time, float fadeoutAlpha){
        if(isFadeout == this.fadedout) return;
        if(isFadeout != this.fadedout && this.lerping) StopAllCoroutines(); 

        if(isFadeout) {
            this.transparentModel.SetActive(true);
            this.solidModel.SetActive(false);
        }

        fadedout = isFadeout;
        lerping = true;
        foreach(Renderer renderer in renderers){
            if(isFadeout){
                Color solidColor = renderer.material.color;
                solidColor.a = 1.0f;

                renderer.material.color = solidColor;
            }

            Color newColor = renderer.material.color;
            if(isFadeout) newColor.a = fadeoutAlpha;
            else newColor.a = 1.0f;

            StartCoroutine(TransitionFade(renderer, renderer.material.color, newColor, time));
        }
    }

    IEnumerator TransitionFade(Renderer tar, Color startColor, Color endColor, float fadeTime){
        float lerpStartTime = Time.time;
        float lerpProgress;

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

        if(!this.fadedout){
            this.solidModel.SetActive(true);
            this.transparentModel.SetActive(false);
        }

        yield break;
    }

}
