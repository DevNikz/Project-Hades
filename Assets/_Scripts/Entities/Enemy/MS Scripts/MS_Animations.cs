using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS_Animations : EnemyAnimation
{
    public override void SetAnimation()
    {
        SetDirection();
        switch (action.Action)
        {
            case 0:
                spriteAnimator.Play("Canter");
                break;
            case 1:
                spriteAnimator.Play("Canter");
                break;
            case 2:
                spriteAnimator.Play("Canter");
                break;
            case 3:
                if (this.getPrevAction() != 3) spriteAnimator.Play("Attack");
                break;
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }
}
