using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator spriteAnimator;
    public Animator vfxAnimator;
    public float rotation = 45f;
    public EntityDirection entityDirection;
    public EntityMovement entityMovement;
    public bool isHit = false;
    public bool isStun = false;
    public bool isDead = false;
    public bool isShooting;
    public float xScale;
    public Vector3 Scale;

    [SerializeField] GameObject obj;
    [SerializeField] private GameObject _deathSprite;
    public EnemyAction action;

    private int prevAction = -1;

    public void OnEnable()
    {
        spriteAnimator = transform.Find("EnemySprite").GetComponent<Animator>();
        vfxAnimator = transform.Find("VfxSprite").GetComponent<Animator>();
        action = this.gameObject.GetComponentInParent<EnemyAction>();
        entityMovement = EntityMovement.Idle;
        spriteAnimator.Play("Idle");
        vfxAnimator.Play("Idle");

        xScale = spriteAnimator.gameObject.transform.localScale.x;
        Scale = spriteAnimator.gameObject.transform.localScale;
        ExtraStart();

        // Debug.Log(action);
        // Debug.Log(action._enemyStats);
    }

    public virtual void ExtraStart() {}

    public virtual void Update() {
        SetDirection();

        if (isDead || (!action.IsPatrolling && !action.IsSearching && !action.IsAttacking)) entityMovement = EntityMovement.Idle;
        else entityMovement = EntityMovement.Strafing;

        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        if (!isHit && !isDead && !isStun && !isShooting) SetAnimation();
        // else Debug.Log("arcane");

        prevAction = action.Action;
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
        spriteAnimator.Play("Move");
    }

    public virtual void SetHit(AttackDirection attackDirection) {
        SetAttackedDirection(attackDirection);
        
        isHit = true;
        spriteAnimator.Play("Hit");
        SFXManager.Instance.PlaySFXAtPosition("Robot_Damaged_SFX", transform.position);
    }

    public virtual void SetStun(AttackDirection attackDirection, float duration)
    {
        SetAttackedDirection(attackDirection);

        isStun = true;
        spriteAnimator.Play("Stun");
        SFXManager.Instance.PlaySFXAtPosition("Enemy_Stun", transform.position);
    }

    public virtual void SetShoot(AttackDirection attackDirection)
    {
        isShooting = true;
        spriteAnimator.Play("Shoot");
        
    }

    public virtual void SetDeath()
    {
        isDead = true;
        action.CancelInvoke();
        action.Cooldown = 10;
        spriteAnimator.Play("Death");
        SFXManager.Instance.PlaySFXAtPosition("Robot_Death_Kill", transform.position);
        this.gameObject.GetComponentInParent<Collider>().enabled = false;
    }

    public void ResetHit() {
        isHit = false;
        spriteAnimator.Play("Idle");
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

    public int getPrevAction() { return prevAction; }

    public void SetAttackedDirection(AttackDirection attackDirection)
    {
        if (attackDirection == AttackDirection.Left) xScale = Math.Abs(xScale) * -1;
        else if(attackDirection == AttackDirection.Right) xScale = Math.Abs(xScale);
        spriteAnimator.gameObject.transform.localScale = new Vector3(xScale, Scale.y, Scale.z);
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
        Vector3 ogScale = _deathSprite.transform.localScale;
        ogScale.x = xScale;
        _deathSprite.transform.localScale = ogScale;
    }

    public void PlayStatusVFX(string name)
    {
        vfxAnimator.Play(name);
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
