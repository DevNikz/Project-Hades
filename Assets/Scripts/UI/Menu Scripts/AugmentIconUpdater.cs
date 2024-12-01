using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class AugmentIconUpdater : MonoBehaviour
{
    [SerializeField] private AugmentScriptable currentAugment;
    [SerializeField] private string unlockEventName;
    private bool isLocked = true;

    private Image targetUIImage;

    private void Start() {
        this.targetUIImage = GetComponent<Image>();
        if(this.targetUIImage == null){
            Destroy(this);
        }

        if(unlockEventName.IsNullOrWhitespace())
            this.isLocked = false;
        else 
            EventBroadcaster.Instance.AddObserver(unlockEventName, this.UnlockAugment);
        
        if(!this.isLocked)
            this.UpdateImage();
    }

    private void OnDestroy() {
        if(!unlockEventName.IsNullOrWhitespace())
            EventBroadcaster.Instance.RemoveActionAtObserver(unlockEventName, this.UnlockAugment);
    }

    public void UnlockAugment(){
        this.isLocked = false;
        this.UpdateImage();
    }

    public void SetAugment(AugmentScriptable augment = null){
        this.currentAugment = augment;
        if(!this.isLocked)
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
