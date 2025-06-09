using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugEAnimation : EnemyAnimation
{
    public override void SetAnimation()
    {
        SetDirection();
        switch (action.Action)
        {
            case 0:
                spriteAnimator.Play("Move");
                break;
            case 1:
                spriteAnimator.Play("Move");
                break;
            case 2:
                spriteAnimator.Play("Move");
                break;
            case 3:
                spriteAnimator.Play("Shoot");
                //SFXManager.Instance.PlaySFX($"Robot_Atk_{UnityEngine.Random.Range(1, 2)}");
                break;
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }
}
