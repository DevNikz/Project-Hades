using System;
using UnityEngine;

public class OHAnimation : EnemyAnimation
{
    public override void SetAnimation()
    {
        SetDirection();
        switch (action.Action)
        {
            case 0:
                spriteAnimator.Play("OH Trotting");
                break;
            case 1:
                spriteAnimator.Play("OH Charging");
                if (getPrevAction() != 1)
                    SFXManager.Instance.PlaySFX("Oxen Harvester Charging");
                break;
            case 2:
                spriteAnimator.Play("OH Trotting");
                break;
            case 3:
                if(this.getPrevAction() != 3) spriteAnimator.Play("Attack");
                break;
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }
}

