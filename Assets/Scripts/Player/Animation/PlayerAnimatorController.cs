using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Android.Gradle;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    // Reference to player skeletal mesh (Bottom and Top)
    // Reference to current player movement, direction, and attack states

    [TitleGroup("References", "General Animator References", Alignment = TitleAlignments.Centered)]
    public bool ShowReferences;
    [ShowIfGroup("ShowReferences")]
    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private Animator skeletalTop;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private Animator skeletalBottom;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private EntityMovement entityMovement;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private EntityState entityState;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private EntityDirection entityDirection;

    void Start() {
        skeletalTop = transform.Find("SpriteT").GetComponent<Animator>();
        skeletalBottom = transform.Find("SpriteB").GetComponent<Animator>();
    }

    public void SetMovement(EntityMovement value) { entityMovement = value; }
    public void SetState(EntityState value) { entityState = value; }
    public void SetDirection(EntityDirection value) { entityDirection = value; }

    void Update() {
        SetAnimBottom(entityMovement, entityDirection);
        SetAnimTop(entityMovement, entityDirection, entityState);
    }

    void SetAnimBottom(EntityMovement move, EntityDirection dir) {
        switch(move, dir) {
            //Idle
            case (EntityMovement.Idle, EntityDirection.East):
            case (EntityMovement.Idle, EntityDirection.NorthEast):
            case (EntityMovement.Idle, EntityDirection.SouthEast):
            case (EntityMovement.Idle, EntityDirection.North):
                skeletalBottom.Play("PlayerIdleB_Right");
                break;
            case (EntityMovement.Idle, EntityDirection.West):
            case (EntityMovement.Idle, EntityDirection.NorthWest):
            case (EntityMovement.Idle, EntityDirection.SouthWest):
            case (EntityMovement.Idle, EntityDirection.South):
                skeletalBottom.Play("PlayerIdleB_Left");
                break;

            //Strafing
            case (EntityMovement.Strafing, EntityDirection.East):
            case (EntityMovement.Strafing, EntityDirection.NorthEast):
            case (EntityMovement.Strafing, EntityDirection.SouthEast):
            case (EntityMovement.Strafing, EntityDirection.North):
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (EntityMovement.Strafing, EntityDirection.West):
            case (EntityMovement.Strafing, EntityDirection.NorthWest):
            case (EntityMovement.Strafing, EntityDirection.SouthWest):
            case (EntityMovement.Strafing, EntityDirection.South):
                skeletalBottom.Play("PlayerRunB_Left");
                break;
        }
    }

    void SetAnimTop(EntityMovement move, EntityDirection dir, EntityState state, Elements element = Elements.None) {
        switch(state, move, dir, element) {
            //None | Idle | None
            case (EntityState.None, EntityMovement.Idle, EntityDirection.East, _):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.NorthEast, _):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.SouthEast, _):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.North, _):
                skeletalTop.Play("PlayerIdleT_Right");
                break;
            case (EntityState.None, EntityMovement.Idle, EntityDirection.West, _):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.NorthWest, _):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.SouthWest, _):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.South, _):
                skeletalTop.Play("PlayerIdleT_Left");
                break;

            //None | Strafing
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.East, _):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.NorthEast, _):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.SouthEast, _):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.North, _):
                skeletalTop.Play("PlayerRunT_Right");
                break;
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.West, _):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.NorthWest, _):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.SouthWest, _):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.South, _):
                skeletalTop.Play("PlayerRunT_Left");
                break;    
        }
    }
}
