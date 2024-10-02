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
    [SerializeReference] private LookDirection entityDirection;

    void Start() {
        skeletalTop = transform.Find("SpriteT").GetComponent<Animator>();
        skeletalBottom = transform.Find("SpriteB").GetComponent<Animator>();
    }

    public void SetMovement(EntityMovement value) { entityMovement = value; }
    public void SetState(EntityState value) { entityState = value; }
    public void SetDirection(LookDirection value) { entityDirection = value; }

    void Update() {
        SetAnimBottom(entityMovement, entityDirection);
        SetAnimTop(entityMovement, entityDirection, entityState);
    }

    void SetAnimBottom(EntityMovement move, LookDirection dir) {
        switch(move, dir) {
            //Idle
            case (EntityMovement.Idle, LookDirection.Right):
                skeletalBottom.Play("PlayerIdleB_Right");
                break;
            case (EntityMovement.Idle, LookDirection.Left):
                skeletalBottom.Play("PlayerIdleB_Left");
                break;

            //Strafing
            case (EntityMovement.Strafing, LookDirection.Right):
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (EntityMovement.Strafing, LookDirection.Left):
                skeletalBottom.Play("PlayerRunB_Left");
                break;
        }
    }

    void SetAnimTop(EntityMovement move, LookDirection dir, EntityState state, Elements element = Elements.None) {
        switch(state, move, dir, element) {
            //None | Idle | None
            case (EntityState.None, EntityMovement.Idle, LookDirection.Right, _):
                skeletalTop.Play("PlayerIdleT_Right");
                break;
            case (EntityState.None, EntityMovement.Idle, LookDirection.Left, _):
                skeletalTop.Play("PlayerIdleT_Left");
                break;

            //None | Strafing
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Right, _):
                skeletalTop.Play("PlayerRunT_Right");
                break;
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Left, _):
                skeletalTop.Play("PlayerRunT_Left");
                break;    
        }
    }
}
