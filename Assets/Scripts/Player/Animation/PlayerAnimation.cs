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

    private void Update() {
        spriteAnimator.gameObject.transform.rotation = Quaternion.Euler(0f, rotation, 0f);

        entityState = PlayerData.entityState;
        entityDirection = PlayerData.entityDirection;

        if(enableAnim == true) if(PlayerData.isAttacking == false) SetAnimation();
    }

    public void SetAnimation() {
        if(PlayerData.entityState == EntityState.Strafing) {
            SetRun();
        }
        else if(PlayerData.entityState == EntityState.Idle) {
            SetIdle();
        }
    }

    public void SetRun() {
        switch(PlayerData.entityDirection) {
            case EntityDirection.East:
                spriteAnimator.Play("MoveRight");
                //movementAnimator.Play("Right");
                break;
            case EntityDirection.NorthEast:
                spriteAnimator.Play("MoveRight");
                //movementAnimator.Play("Right");
                break;
            case EntityDirection.SouthEast:
                spriteAnimator.Play("MoveRight");
                //movementAnimator.Play("Right");
                break;
            case EntityDirection.West:
                spriteAnimator.Play("MoveLeft");
                //movementAnimator.Play("Left");
                break;
            case EntityDirection.NorthWest:
                spriteAnimator.Play("MoveLeft");
                //movementAnimator.Play("Left");
                break;
            case EntityDirection.SouthWest:
                spriteAnimator.Play("MoveLeft");
                //movementAnimator.Play("Left");
                break;
            case EntityDirection.North:
                spriteAnimator.Play("MoveUp");
                //movementAnimator.Play("Up");
                break;
            case EntityDirection.South:
                spriteAnimator.Play("MoveDown");
                //movementAnimator.Play("Down");
                break;
        }
    }

    public void SetIdle() {
        switch(PlayerData.entityDirection) {
            case EntityDirection.East:
                spriteAnimator.Play("IdleLeft");
                //movementAnimator.Play("IRight");
                break;
            case EntityDirection.NorthEast:
                spriteAnimator.Play("IdleLeft");
                //movementAnimator.Play("IRight");
                break;
            case EntityDirection.SouthEast:
                spriteAnimator.Play("IdleLeft");
                //movementAnimator.Play("IRight");
                break;
            case EntityDirection.West:
                spriteAnimator.Play("IdleRight");
                //movementAnimator.Play("ILeft");
                break;
            case EntityDirection.NorthWest:
                spriteAnimator.Play("IdleRight");
                //movementAnimator.Play("ILeft");
                break;
            case EntityDirection.SouthWest:
                spriteAnimator.Play("IdleRight");
                //movementAnimator.Play("ILeft");
                break;
            case EntityDirection.North:
                spriteAnimator.Play("IdleLeft");
                //movementAnimator.Play("IUp");
                break;
            case EntityDirection.South:
                spriteAnimator.Play("IdleRight");
                //movementAnimator.Play("IDown");
                break;
        }
    }
}
