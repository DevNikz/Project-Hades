using System;
using UnityEngine;

public class LCAnimation : EnemyAnimation
{
    public bool run;
    public float attackTime;
    private bool isDead = false;

    public override void ExtraStart()
    {
        spriteAnimator.SetFloat("ComboSpeed", 1 / action.FireRate);
    }

    public override void Update()
    {
        SetDirection();
        if (isDead) entityMovement = EntityMovement.Idle;
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        if (!isHit && !isStun && !isDead) SetAnimation();
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
                spriteAnimator.Play("Combo1");
                break;
            case 4:
                spriteAnimator.Play("Combo2");
                break;
            case 5:
                spriteAnimator.Play("Combo3");
                break;
            case 6:
                break;
            case 10:
                spriteAnimator.Play("Death");
                break;
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }

    public void SetStun(AttackDirection attackDirection)
    {
        if (attackDirection == AttackDirection.Left) xScale = Math.Abs(xScale) * -1;
        else xScale = Math.Abs(xScale);

        isStun = true;
        spriteAnimator.gameObject.transform.localScale = new Vector3(xScale, Scale.y, Scale.z);
        spriteAnimator.Play("Stun");
        ResetHit();
    }

    public override void SetDeath()
    {
        isDead = true;
        action.CancelInvoke();
        spriteAnimator.Play("Death");
    }

    public override void ResetAnim()
    {
        isHit = false;
        isStun = false;
        action.agent.isStopped = false;
    }
}

