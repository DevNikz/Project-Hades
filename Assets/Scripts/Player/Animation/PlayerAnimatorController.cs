using Sirenix.OdinInspector;
using Unity.Android.Gradle;
using Unity.Collections.LowLevel.Unsafe;
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

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private Elements elements;

    void Start() {
        skeletalTop = transform.Find("SpriteT").GetComponent<Animator>();
        skeletalBottom = transform.Find("SpriteB").GetComponent<Animator>();
    }

    public void SetMovement(EntityMovement value) { entityMovement = value; }
    public void SetState(EntityState value) { entityState = value; }
    public void SetDirection(LookDirection value) { entityDirection = value; }
    public void SetElements(Elements value) { elements = value; }

    void Update() {
        SetAnimBottom(entityMovement, entityDirection, entityState, elements);
        SetAnimTop(entityMovement, entityDirection, entityState, elements);
    }

    public void PlayAnimation(int counter, AttackDirection dir, Elements element) {
        switch(counter, dir, element) {
            case (1, AttackDirection.Right, Elements.Earth): 
                skeletalTop.Play("Earth_T_R_1");
                skeletalBottom.Play("Earth_B_R_1");
                break;
            case (1, AttackDirection.Left, Elements.Earth):
                skeletalTop.Play("Earth_T_L_1");
                skeletalBottom.Play("Earth_B_L_1"); 
                break;

            case (2, AttackDirection.Right, Elements.Earth): 
                skeletalTop.Play("Earth_T_R_2");
                skeletalBottom.Play("Earth_B_R_2");
                break;
            case (2, AttackDirection.Left, Elements.Earth):
                skeletalTop.Play("Earth_T_L_2");
                skeletalBottom.Play("Earth_B_L_2");
                break;

            case (3, AttackDirection.Right, Elements.Earth):
                skeletalTop.Play("Earth_T_R_3");
                skeletalBottom.Play("Earth_B_R_3"); 
                break;
            case (3, AttackDirection.Left, Elements.Earth):
                skeletalTop.Play("Earth_T_L_3");
                skeletalBottom.Play("Earth_B_L_3"); 
                break;
        }
    }

    void SetAnimBottom(EntityMovement move, LookDirection dir, EntityState state, Elements element) {
        switch(move, dir, state, element) {
            //Idle
            case (EntityMovement.Idle, LookDirection.Right, EntityState.None, Elements.None):
                skeletalBottom.Play("PlayerIdleB_Right");
                break;
            case (EntityMovement.Idle, LookDirection.Left, EntityState.None, Elements.None):
                skeletalBottom.Play("PlayerIdleB_Left");
                break;

            //Strafing
            case (EntityMovement.Strafing, LookDirection.Right, EntityState.None, Elements.None):
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (EntityMovement.Strafing, LookDirection.Left, EntityState.None, Elements.None):
                skeletalBottom.Play("PlayerRunB_Left");
                break;
        }
    }

    void SetAnimTop(EntityMovement move, LookDirection dir, EntityState state, Elements element) {
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
