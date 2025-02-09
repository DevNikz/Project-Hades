using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LCAnimation : EnemyAnimation
{
    public override void ExtraStart()
    {
        spriteAnimator.SetFloat("ComboSpeed", 1 / action.AttackRate);
    }

    public override void SetAnimation()
    {
        switch (action.Action)
        {
            case 0:
                spriteAnimator.Play("Run");
                break;
            case 1:
                spriteAnimator.Play("Run");
                break;
            case 2:
                spriteAnimator.Play("Run");
                break;
            case 3:
                //spriteAnimator.Play("Combo1");
                break;
            case 4:
                spriteAnimator.Play("Combo2"); //combo1
                break;
            case 5:
                spriteAnimator.Play("Combo3"); //combo2
                break;
            case 6:
                spriteAnimator.Play("Combo2"); //combo3
                break;
            case 10:
                spriteAnimator.Play("Death");
                break;
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }
}

