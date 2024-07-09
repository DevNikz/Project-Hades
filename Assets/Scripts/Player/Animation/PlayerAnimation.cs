using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator spriteAnimator;
    public float rotation = 45f;
    public EntityDirection entityDirection;
    public EntityState entityState;
    public bool enableAnim;

    private void Start() {
        spriteAnimator = transform.Find("Sprite").GetComponent<Animator>();
    }

    void OnEnable() {
        enableAnim = true;
    }

    void OnDisable() {
        enableAnim = false;
    }

    private void FixedUpdate() {
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);

        entityState = PlayerData.entityState;
        entityDirection = PlayerData.entityDirection;

        if(enableAnim == true) if(PlayerData.isAttacking == false) SetAnimation();
    }

    public EntityState GetEntityState() {
        return entityState;
    }

    public void SetAnimation() {
        if(entityState == EntityState.Strafing) {
            SetRun();
        }
        else if(entityState == EntityState.Idle) {
            SetIdle();
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

    public void SetIdle() {
        switch(entityDirection) {
            case EntityDirection.East:
                spriteAnimator.Play("IdleLeft");
                break;
            case EntityDirection.NorthEast:
                spriteAnimator.Play("IdleLeft");
                break;
            case EntityDirection.SouthEast:
                spriteAnimator.Play("IdleLeft");
                break;
            case EntityDirection.West:
                spriteAnimator.Play("IdleRight");
                break;
            case EntityDirection.NorthWest:
                spriteAnimator.Play("IdleRight");
                break;
            case EntityDirection.SouthWest:
                spriteAnimator.Play("IdleRight");
                break;
            case EntityDirection.North:
                spriteAnimator.Play("IdleLeft");
                break;
            case EntityDirection.South:
                spriteAnimator.Play("IdleRight");
                break;
        }
    }
}
