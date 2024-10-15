using System;
using UnityEngine;

public class OHAnimation : MonoBehaviour
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

    private void Start()
    {
        spriteAnimator = transform.Find("EnemySprite").GetComponent<Animator>();
        Enemy = this.gameObject.GetComponentInParent<EnemyAction>();
    }

    private void Update()
    {
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        spriteAnimator.SetBool("PFound", run);
        if (!isHit && !isStun) SetAnimation();
    }

    public void SetAnimation()
    {
        switch (Enemy.Action)
        {
            case 0:
                spriteAnimator.Play("OH Trotting");
                break;
            case 1:
                spriteAnimator.Play("Attack");
                break;
            case 2:
                spriteAnimator.Play("OH Charging");
                break;
            default:
                spriteAnimator.Play("Idle");
                break;
        }
    }

    public void Hurt()
    {
        spriteAnimator.Play("Hurt");
        isHit = true;
        Invoke("UnHurt", 1);
    }

    public void UnHurt()
    {
        isHit = false;
    }

    public void Stun(float time)
    {
        spriteAnimator.Play("Stun");
        isStun = true;
        Invoke("UnStun", time);
    }

    public void UnStun()
    {
        isStun = false;
    }

    public void ResetHit()
    {
        Invoke(nameof(ResetAnim), timer);
    }

    void ResetAnim()
    {
        isHit = false;
    }
}

