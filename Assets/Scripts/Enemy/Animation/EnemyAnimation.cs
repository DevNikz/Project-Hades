using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator spriteAnimator;
    public float rotation = 45f;
    public EntityDirection entityDirection;
    public EntityMovement entityMovement;
    public bool isHit = false;
    public bool isStun = false;
    public bool isShooting;
    public float timer;

    [SerializeField] GameObject obj;
    private EnemyAction action;


    public virtual void Start() {
        spriteAnimator = transform.Find("EnemySprite").GetComponent<Animator>();
        action = obj.GetComponent<EnemyAction>();
        entityMovement = EntityMovement.Idle;
    }

    public virtual void Update() {
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);

        if (!action.isPatrolling && !action.isSearching && !action.isAttacking) entityMovement = EntityMovement.Idle;
        else entityMovement = EntityMovement.Strafing;

        if (isHit == false && isShooting == false) SetAnimation();
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
        switch(entityDirection) {
            case EntityDirection.East:
                spriteAnimator.Play("MoveRight");
                break;
            case EntityDirection.NorthEast:
                spriteAnimator.Play("MoveRight");
                break;
            case EntityDirection.SouthEast:
                spriteAnimator.Play("MoveRight");
                break;
            case EntityDirection.West:
                spriteAnimator.Play("MoveLeft");
                break;
            case EntityDirection.NorthWest:
                spriteAnimator.Play("MoveLeft");
                break;
            case EntityDirection.SouthWest:
                spriteAnimator.Play("MoveLeft");
                break;
            case EntityDirection.North:
                spriteAnimator.Play("MoveUp");
                break;
            case EntityDirection.South:
                spriteAnimator.Play("MoveDown");
                break;
        }
    }

    public virtual void SetHit(AttackDirection attackDirection) {
        isHit = true;
        switch(attackDirection) {
            case AttackDirection.Right:
                spriteAnimator.Play("HitRight");
                ResetHit();
                break;
            case AttackDirection.Left:
                spriteAnimator.Play("HitLeft");
                ResetHit();
                break;
        }
    }

    public virtual void SetShoot(AttackDirection attackDirection)
    {
        isShooting = true;
        switch(attackDirection)
        {
            case AttackDirection.Right:
                spriteAnimator.Play("ShootRight");
                //ResetHit();
                break;
            case AttackDirection.Left:
                spriteAnimator.Play("ShootLeft");
                //ResetHit();
                break;
        }
    }

    public virtual void SetDeath()
    {
        spriteAnimator.Play("BugeDeath");
    }

    public void ResetHit() {
        Invoke(nameof(ResetAnim), timer);
    }

    public virtual void ResetAnim() {
        isHit = false;
        //isShooting = false;
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
