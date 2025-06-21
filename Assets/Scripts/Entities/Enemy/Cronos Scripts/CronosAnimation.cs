using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CronosAnimation : EnemyAnimation
{
    public override void SetAnimation()
    {
        SetDirection();
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
                if (this.getPrevAction() != 3)
                {
                    spriteAnimator.Play("Reap");
                    SFXManager.Instance.PlaySFXAtPosition("CronosSwing", transform.position);
                }
                break;
            case 4:
                if (this.getPrevAction() != 4)
                {
                    spriteAnimator.Play("Dash");
                    SFXManager.Instance.PlaySFXAtPosition("CronosCharge", transform.position);
                }
                break;
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }
}
