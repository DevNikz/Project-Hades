using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireChargeDisplayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _filledSprite;
    public void SetFill(float chargeTime, float fullchargeTime)
    {
        float fillAmount = fullchargeTime > 0 ? chargeTime / fullchargeTime : 1;
        if(fillAmount > 1) fillAmount = 1;
        _filledSprite.size = new Vector2(fillAmount, fillAmount);
    }
}
