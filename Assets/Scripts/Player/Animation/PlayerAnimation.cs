using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator movementAnimator;
    private Animator attackAnimator;
    public float rotation = 45f;
    public EntityDirection entityDirection;
    public EntityState entityState;

    private void Start() {
        movementAnimator = transform.Find("Movement").GetComponent<Animator>();
        attackAnimator = transform.Find("Attack").GetComponent<Animator>();
    }

    private void Update() {
        movementAnimator.gameObject.transform.rotation = Quaternion.Euler(0f,rotation,0f);
        attackAnimator.gameObject.transform.rotation = Quaternion.Euler(0f,rotation,0f);

        if(PlayerData.entityState != EntityState.BasicAttack) SetAnimation();
        entityDirection = PlayerData.entityDirection;
        entityState = PlayerData.entityState;
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
                movementAnimator.Play("Right");
                break;
            case EntityDirection.NorthEast:
                movementAnimator.Play("Right");
                break;
            case EntityDirection.SouthEast:
                movementAnimator.Play("Right");
                break;
            case EntityDirection.West:
                movementAnimator.Play("Left");
                break;
            case EntityDirection.NorthWest:
                movementAnimator.Play("Left");
                break;
            case EntityDirection.SouthWest:
                movementAnimator.Play("Left");
                break;
            case EntityDirection.North:
                movementAnimator.Play("Up");
                break;
            case EntityDirection.South:
                movementAnimator.Play("Down");
                break;
        }
    }

    public void SetIdle() {
        switch(PlayerData.entityDirection) {
            case EntityDirection.East:
                movementAnimator.Play("IRight");
                break;
            case EntityDirection.NorthEast:
                movementAnimator.Play("IRight");
                break;
            case EntityDirection.SouthEast:
                movementAnimator.Play("IRight");
                break;
            case EntityDirection.West:
                movementAnimator.Play("ILeft");
                break;
            case EntityDirection.NorthWest:
                movementAnimator.Play("ILeft");
                break;
            case EntityDirection.SouthWest:
                movementAnimator.Play("ILeft");
                break;
            case EntityDirection.North:
                movementAnimator.Play("IUp");
                break;
            case EntityDirection.South:
                movementAnimator.Play("IDown");
                break;
        }
    }
}
