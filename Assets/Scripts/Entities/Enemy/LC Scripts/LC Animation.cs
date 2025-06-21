using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LCAnimation : EnemyAnimation
{
    public override void ExtraStart()
    {
        spriteAnimator.SetFloat("ComboSpeed", 0.7f);
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
                spriteAnimator.Play("Run");
                break;
            case 4:
                spriteAnimator.Play("Combo1"); //combo1
                if (getPrevAction() != 4)
                    SFXManager.Instance.PlaySFXAtPosition("LCMelee", transform.position);
                break;
            case 5:
                spriteAnimator.Play("Combo2"); //combo2
                if (getPrevAction() != 5)
                    SFXManager.Instance.PlaySFXAtPosition("LCMelee", transform.position);
                break;
            case 6:
                spriteAnimator.Play("Combo3"); //combo3
                if (getPrevAction() != 6)
                    SFXManager.Instance.PlaySFXAtPosition("LCMelee", transform.position);
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

