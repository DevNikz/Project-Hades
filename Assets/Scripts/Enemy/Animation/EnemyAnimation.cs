using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator spriteAnimator;
    public float rotation = 45f;
    public EntityDirection entityDirection;
    public EntityMovement entityMovement;
    public bool isHit;
    public bool isShooting;
    public float timer;

    [SerializeField] GameObject obj;
    private EnemyAction action;


    private void Start() {
        spriteAnimator = transform.Find("EnemySprite").GetComponent<Animator>();
        action = obj.GetComponent<EnemyAction>();
        entityMovement = EntityMovement.Idle;
    }

    private void Update() {
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);

        if (!action.isPatrolling && !action.isSearching && !action.isAttacking) entityMovement = EntityMovement.Idle;
        else entityMovement = EntityMovement.Strafing;

        if (isHit == false && isShooting == false) SetAnimation();
    }

    public void SetAnimation() {
        if(entityMovement == EntityMovement.Strafing) {
            SetRun();
        }
        else
        {
            spriteAnimator.Play("Idle");
        }
    }

    public void SetRun() {
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

    public void SetHit(AttackDirection attackDirection) {
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

    public void SetShoot(AttackDirection attackDirection)
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

    public void SetDeath()
    {
        spriteAnimator.Play("BugeDeath");
    }

    public void ResetHit() {
        Invoke(nameof(ResetAnim), timer);
    }

    void ResetAnim() {
        isHit = false;
        //isShooting = false;
    }
}
