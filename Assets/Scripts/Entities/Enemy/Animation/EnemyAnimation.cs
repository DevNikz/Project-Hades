using System;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator spriteAnimator;
    public float rotation = 45f;
    public EntityDirection entityDirection;
    public EntityMovement entityMovement;
    public bool isHit = false;
    public bool isStun = false;
    public bool isDead = false;
    public bool isShooting;
    public float timer;
    public float xScale;
    public Vector3 Scale;

    [SerializeField] GameObject obj;
    public EnemyAction action;


    public void Start() {
        spriteAnimator = transform.Find("EnemySprite").GetComponent<Animator>();
        action = this.gameObject.GetComponentInParent<EnemyAction>();
        entityMovement = EntityMovement.Idle;
        spriteAnimator.Play("Idle");

        xScale = spriteAnimator.gameObject.transform.localScale.x;
        Scale = spriteAnimator.gameObject.transform.localScale;
        ExtraStart();
    }

    public virtual void ExtraStart() {}

    public virtual void Update() {
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        SetDirection();

        if (!action.IsPatrolling && !action.IsSearching && !action.IsAttacking) entityMovement = EntityMovement.Idle;
        else entityMovement = EntityMovement.Strafing;

        if (isHit == false && isShooting == false && isStun == false) SetAnimation();
    }

    public virtual void SetAnimation() {
        if(entityMovement == EntityMovement.Strafing) {
            SetRun();
        }
        else
        {
            spriteAnimator.Play("Idle");
        }
    }

    public virtual void SetRun() {
        spriteAnimator.Play("MoveRight");
    }

    public virtual void SetHit(AttackDirection attackDirection) {
        if (attackDirection == AttackDirection.Right) xScale = Math.Abs(xScale) * -1;
        else xScale = Math.Abs(xScale);
        action.Agent.isStopped = true;
        isHit = true;
        spriteAnimator.gameObject.transform.localScale = new Vector3(xScale, Scale.y, Scale.z);
        spriteAnimator.Play("Hit");
        Invoke(nameof(ResetHit), timer);
    }

    public virtual void SetStun(AttackDirection attackDirection, float duration)
    {
        if (attackDirection == AttackDirection.Left) xScale = Math.Abs(xScale) * -1;
        else xScale = Math.Abs(xScale);

        isStun = true;
        spriteAnimator.gameObject.transform.localScale = new Vector3(xScale, Scale.y, Scale.z);
        spriteAnimator.Play("Stun");
        action.CancelInvoke();

        action.Cooldown = duration;
        Invoke(nameof(ResetStun), duration);
    }

    public virtual void SetShoot(AttackDirection attackDirection)
    {
        isShooting = true;
        spriteAnimator.Play("ShootRight");
    }

    public virtual void SetDeath()
    {
        isDead = true;
        action.CancelInvoke();
        spriteAnimator.Play("Death");
    }

    public void ResetHit() {
        isHit = false;
    }

    public void ResetStun()
    {
        isStun = false;
        spriteAnimator.Play("Idle");
    }

    public virtual void ResetAnim() {
        isHit = false;
        isStun = false;
        spriteAnimator.Play("Idle");
        //isShooting = false;
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

    public EntityDirection IsoCompass(float x, float z)
    {
        //North
        if (x == 0 && (z <= 1 && z > 0))
        {
            return EntityDirection.North;
        }

        //North East
        else if ((x <= 1 && x > 0) && (z <= 1 && z > 0))
        {
            return EntityDirection.NorthEast;
        }

        //East
        else if ((x <= 1 && x > 0) && z == 0)
        {
            return EntityDirection.East;
        }

        //South East
        else if ((x <= 1 && x > 0) && (z >= -1 && z < 0))
        {
            return EntityDirection.SouthEast;
        }

        //South
        else if (x == 0 && (z >= -1 && z < 0))
        {
            return EntityDirection.South;
        }

        //South West
        else if ((x >= -1 && x < 0) && (z >= -1 && z < 0))
        {
            return EntityDirection.SouthWest;
        }

        //West
        else if ((x >= -1 && x < 0) && z == 0)
        {
            return EntityDirection.West;
        }

        //North West
        else if ((x >= -1 && x < 0) && (z <= 1 && z > 0))
        {
            return EntityDirection.NorthWest;
        }

        else return EntityDirection.East;
    }
}
