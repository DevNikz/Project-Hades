using System;
using UnityEngine;

public class LCAnimation : MonoBehaviour
{
    private Animator spriteAnimator;
    public float rotation = 45f;
    public EntityDirection entityDirection;
    public EntityMovement entityMovement;
    public bool isHit;
    public bool isStun;
    public float timer;
    public bool run;
    public float attackTime;
    private EnemyAction Enemy;
    private float xScale;
    private Vector3 Scale;

    private void Start()
    {
        spriteAnimator = transform.Find("EnemySprite").GetComponent<Animator>();
        Enemy = this.gameObject.GetComponentInParent<EnemyAction>();

        xScale = spriteAnimator.gameObject.transform.localScale.x;
        Scale = spriteAnimator.gameObject.transform.localScale;
        spriteAnimator.SetFloat("ComboSpeed", 1 / Enemy.FireRate);
    }

    private void Update()
    {
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        if (!isHit && !isStun) SetAnimation();
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

    public void SetAnimation()
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
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }

    public void SetHit(AttackDirection attackDirection)
    {
        if (attackDirection == AttackDirection.Left) xScale = Math.Abs(xScale) * -1;
        else xScale = Math.Abs(xScale);

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

    public void ResetHit()
    {
        Invoke(nameof(ResetAnim), timer);
    }

    void ResetAnim()
    {
        isHit = false;
        isStun = false;
    }

    private EntityDirection IsoCompass(float x, float z)
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

