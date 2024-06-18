using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    public float rotation = 45f;
    public EntityDirection entityDirection;
    public EntityState entityState;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        this.transform.rotation = Quaternion.Euler(0f,rotation,0f);
        SetDirection();
        entityDirection = PlayerData.entityDirection;
        entityState = PlayerData.entityState;
    }

    public void SetDirection() {
        if(PlayerData.entityState == EntityState.Strafing) {
            SetRun();
        }
        else {
            SetIdle();
        }
    }

    public void SetRun() {
        switch(PlayerData.entityDirection) {
            case EntityDirection.East:
                anim.Play("Right");
                break;
            case EntityDirection.West:
                anim.Play("Left");
                break;
            case EntityDirection.North:
                anim.Play("Up");
                break;
            case EntityDirection.South:
                anim.Play("Down");
                break;
        }
    }

    public void SetIdle() {
        switch(PlayerData.entityDirection) {
            case EntityDirection.East:
                anim.Play("IRight");
                break;
            case EntityDirection.West:
                anim.Play("ILeft");
                break;
            case EntityDirection.North:
                anim.Play("IUp");
                break;
            case EntityDirection.South:
                anim.Play("IDown");
                break;
        }
    }
}
