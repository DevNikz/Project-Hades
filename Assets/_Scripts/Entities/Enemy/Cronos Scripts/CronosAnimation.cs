using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CronosAnimation : EnemyAnimation
{
    [SerializeField] Animator atkvfxAnimator;
    public Vector3 atkvfxSpritePos;

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
                    atkvfxAnimator.Play("CronosAtkVFX1");
                    SFXManager.Instance.PlaySFXAtPosition("CronosSwing", transform.position);
                }
                break;
            case 4:
                if (this.getPrevAction() != 4)
                {
                    spriteAnimator.Play("Dash");
                    atkvfxAnimator.Play("CronosAtkVFX2");
                    SFXManager.Instance.PlaySFXAtPosition("CronosCharge", transform.position);
                }
                break;
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }
}
