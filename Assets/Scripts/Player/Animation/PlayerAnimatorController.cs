using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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

    public void UpdateStates(EntityMovement move, EntityDirection dir, EntityState state = EntityState.None) {
        entityMovement = move;
        entityState = state;
        entityDirection = dir;
    }

    public void SetAnimBottom(EntityMovement move, EntityDirection dir) {
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

            //Dash
        }
    }

    public void SetAnimTop(EntityMovement move, EntityDirection dir, EntityState state = EntityState.None) {
        switch(state, move, dir) {
            //None | Idle
            case (EntityState.None, EntityMovement.Idle, EntityDirection.East):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.NorthEast):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.SouthEast):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.North):
                skeletalTop.Play("PlayerIdleT_Right");
                break;
            case (EntityState.None, EntityMovement.Idle, EntityDirection.West):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.NorthWest):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.SouthWest):
            case (EntityState.None, EntityMovement.Idle, EntityDirection.South):
                skeletalTop.Play("PlayerIdleT_Left");
                break;

            //None | Strafing
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.East):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.NorthEast):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.SouthEast):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.North):
                skeletalTop.Play("PlayerRunT_Right");
                break;
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.West):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.NorthWest):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.SouthWest):
            case (EntityState.None, EntityMovement.Strafing, EntityDirection.South):
                skeletalTop.Play("PlayerRunT_Left");
                break;

            //Attack | Idle (Too many types | I'll update it later lol)
            case (EntityState.Attack, EntityMovement.Idle, EntityDirection.East):
            case (EntityState.Attack, EntityMovement.Idle, EntityDirection.NorthEast):
            case (EntityState.Attack, EntityMovement.Idle, EntityDirection.SouthEast):
            case (EntityState.Attack, EntityMovement.Idle, EntityDirection.North):
                break;
            case (EntityState.Attack, EntityMovement.Idle, EntityDirection.West):
            case (EntityState.Attack, EntityMovement.Idle, EntityDirection.NorthWest):
            case (EntityState.Attack, EntityMovement.Idle, EntityDirection.SouthWest):
            case (EntityState.Attack, EntityMovement.Idle, EntityDirection.South):
                break;

            //Attack | Strafing
            case (EntityState.Attack, EntityMovement.Strafing, EntityDirection.East):
            case (EntityState.Attack, EntityMovement.Strafing, EntityDirection.NorthEast):
            case (EntityState.Attack, EntityMovement.Strafing, EntityDirection.SouthEast):
            case (EntityState.Attack, EntityMovement.Strafing, EntityDirection.North):
                break;
            case (EntityState.Attack, EntityMovement.Strafing, EntityDirection.West):
            case (EntityState.Attack, EntityMovement.Strafing, EntityDirection.NorthWest):
            case (EntityState.Attack, EntityMovement.Strafing, EntityDirection.SouthWest):
            case (EntityState.Attack, EntityMovement.Strafing, EntityDirection.South):
                break;                     
        }
    }
}
