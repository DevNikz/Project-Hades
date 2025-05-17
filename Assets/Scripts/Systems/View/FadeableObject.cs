using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeableObject : MonoBehaviour
{
    [SerializeField] private GameObject solidModel;
    [SerializeField] private GameObject transparentModel;
    [SerializeField] private string _colorFieldName = "_BaseColor";
    [SerializeField, HideInInspector] private Renderer[] renderers;

    private MaskObject maskObjectScriptRef;

    bool lerping = false;
    bool fadedout = false;

    void OnEnable(){
        if(this.solidModel == null || this.transparentModel == null){
            Destroy(this);
        }

        renderers = this.transparentModel.GetComponentsInChildren<Renderer>(true);
        if(renderers.Length == 0){
            Destroy(this);
        }

        this.transparentModel.SetActive(false);
        this.solidModel.SetActive(true);
        maskObjectScriptRef = GameObject.Find("Player")?.GetComponent<MaskObject>();
        if(maskObjectScriptRef != null){
            maskObjectScriptRef.fadeableObjects.Add(this.gameObject);
        } 
    }

    void OnDisable(){
        if(maskObjectScriptRef != null)
            maskObjectScriptRef.fadeableObjects.Remove(this.gameObject);
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

            if (!renderer.material.HasColor(_colorFieldName))
            {
                Debug.LogWarning($"[WARN]: {this}, {renderer.gameObject}, {renderer.material} missing \"{_colorFieldName}\" property");
                transparentModel.SetActive(false);
                if (!isFadeout)
                    solidModel.SetActive(true);
                continue;
            }
            
            

            if (isFadeout)
            {
                Color solidColor = renderer.material.GetColor(_colorFieldName);
                solidColor.a = 1.0f;

                renderer.material.SetColor(_colorFieldName, solidColor);
            }

            Color newColor = renderer.material.GetColor(_colorFieldName);
            if(isFadeout) newColor.a = fadeoutAlpha;
            else newColor.a = 1.0f;

            StartCoroutine(TransitionFade(renderer, renderer.material.GetColor(_colorFieldName), newColor, time));
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
                    tar.material.SetColor(_colorFieldName, endColor);
                    lerping = false;
                } else {
                    tar.material.SetColor(_colorFieldName, Color.Lerp(startColor, endColor, lerpProgress/fadeTime));
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
