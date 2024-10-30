using System;
using UnityEngine;

public class LCAnimation : EnemyAnimation
{
    public bool run;
    public float attackTime;
    private EnemyAction Enemy;
    private float xScale;
    private Vector3 Scale;
    private bool isDead = false;

    public override void Start()
    {
        spriteAnimator = transform.Find("EnemySprite").GetComponent<Animator>();
        Enemy = this.gameObject.GetComponentInParent<EnemyAction>();

        xScale = spriteAnimator.gameObject.transform.localScale.x;
        Scale = spriteAnimator.gameObject.transform.localScale;
        spriteAnimator.SetFloat("ComboSpeed", 1 / Enemy.FireRate);
    }

    public override void Update()
    {
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        if (!isHit && !isStun && !isDead) SetAnimation();
    }

    public void SetDirection()
    {
        entityDirection = IsoCompass(this.transform.forward.x, this.transform.forward.z);

        switch (entityDirection)
        {
            case EntityDirection.East:
            case EntityDirection.NorthEast:
            case EntityDirection.SouthEast:
            case EntityDirection.North:
                xScale = Math.Abs(xScale);
                break;
            case EntityDirection.West:
            case EntityDirection.NorthWest:
            case EntityDirection.SouthWest:
            case EntityDirection.South:
                xScale = Math.Abs(xScale) * -1;
                break;
        }

        spriteAnimator.gameObject.transform.localScale = new Vector3(xScale, Scale.y, Scale.z);
    }

    public override void SetAnimation()
    {
        SetDirection();
        switch (Enemy.Action)
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

    public override void SetHit(AttackDirection attackDirection)
    {
        if (attackDirection == AttackDirection.Right) xScale = Math.Abs(xScale) * -1;
        else xScale = Math.Abs(xScale);
        Enemy.agent.isStopped = true;
        isHit = true;
        spriteAnimator.gameObject.transform.localScale = new Vector3(xScale, Scale.y, Scale.z);
        spriteAnimator.Play("Hit");
        ResetHit();
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
        Enemy.SetAction(10);
        spriteAnimator.Play("Death");
        this.enabled = false;
        Enemy.agent.isStopped = true;
    }

    public override void ResetAnim()
    {
        isHit = false;
        isStun = false;
        Enemy.agent.isStopped = false;
    }
}

