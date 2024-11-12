using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentIconUpdater : MonoBehaviour
{
    [SerializeField] private AugmentScriptable currentAugment;

    private Image targetUIImage;

    private void Start() {
        this.targetUIImage = GetComponent<Image>();
        this.UpdateImage();
        if(this.targetUIImage == null){
            Destroy(this);
        }
    }

    public void SetAugment(AugmentScriptable augment = null){
        this.currentAugment = augment;
        this.UpdateImage();
    }

    private void UpdateImage(){
        if(this.targetUIImage != null && this.currentAugment != null){
            this.targetUIImage.sprite = this.currentAugment.augmentIcon;
        } 
    }

    public AugmentScriptable GetAugment(){
        return this.currentAugment;
    }
    
}
