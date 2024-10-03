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

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private Elements elements;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private Dashing dashing;

    void Start() {
        skeletalTop = transform.Find("SpriteT").GetComponent<Animator>();
        skeletalBottom = transform.Find("SpriteB").GetComponent<Animator>();
    }

    public void SetMovement(EntityMovement value) { entityMovement = value; }
    public void SetState(EntityState value) { entityState = value; }
    public void SetDirection(LookDirection value) { entityDirection = value; }
    public void SetElements(Elements value) { elements = value; }
    public void SetDashing(Dashing value) { dashing = value; }

    void Update() {
        if(dashing == Dashing.Yes) Debug.Log("Dashing!");
        SetAnimBottom(entityMovement, entityDirection, entityState, elements, dashing);
        SetAnimTop(entityMovement, entityDirection, entityState, elements, dashing);
    }

    public void PlayAnimation(int counter, AttackDirection dir, Elements element) {
        switch(counter, dir, element, entityMovement, entityDirection) {
            //Idle
            case (1, AttackDirection.Right, Elements.Earth, EntityMovement.Idle, _): 
                skeletalTop.Play("Earth_T_R_1");
                skeletalBottom.Play("Earth_B_R_1");
                break;
            case (1, AttackDirection.Left, Elements.Earth, EntityMovement.Idle, _):
                skeletalTop.Play("Earth_T_L_1");
                skeletalBottom.Play("Earth_B_L_1"); 
                break;

            case (2, AttackDirection.Right, Elements.Earth, EntityMovement.Idle, _): 
                skeletalTop.Play("Earth_T_R_2");
                skeletalBottom.Play("Earth_B_R_2");
                break;
            case (2, AttackDirection.Left, Elements.Earth, EntityMovement.Idle, _):
                skeletalTop.Play("Earth_T_L_2");
                skeletalBottom.Play("Earth_B_L_2");
                break;

            case (3, AttackDirection.Right, Elements.Earth, EntityMovement.Idle, _):
                skeletalTop.Play("Earth_T_R_3");
                skeletalBottom.Play("Earth_B_R_3"); 
                break;
            case (3, AttackDirection.Left, Elements.Earth, EntityMovement.Idle, _):
                skeletalTop.Play("Earth_T_L_3");
                skeletalBottom.Play("Earth_B_L_3"); 
                break;

            //Strafing | Right
            case (1, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Right): 
                skeletalTop.Play("Earth_T_R_1");
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (1, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Right):
                skeletalTop.Play("Earth_T_L_1");
                skeletalBottom.Play("PlayerRunB_Right");
                break;

            case (2, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Right): 
                skeletalTop.Play("Earth_T_R_2");
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (2, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Right):
                skeletalTop.Play("Earth_T_L_2");
                skeletalBottom.Play("PlayerRunB_Right");
                break;

            case (3, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Right):
                skeletalTop.Play("Earth_T_R_3");
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (3, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Right):
                skeletalTop.Play("Earth_T_L_3");
                skeletalBottom.Play("PlayerRunB_Right");
                break;

            //Strafing | Left
            case (1, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Left): 
                skeletalTop.Play("Earth_T_R_1");
                skeletalBottom.Play("PlayerRunB_Left");
                break;
            case (1, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Left):
                skeletalTop.Play("Earth_T_L_1");
                skeletalBottom.Play("PlayerRunB_Left");
                break;

            case (2, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Left): 
                skeletalTop.Play("Earth_T_R_2");
                skeletalBottom.Play("PlayerRunB_Left");
                break;
            case (2, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Left):
                skeletalTop.Play("Earth_T_L_2");
                skeletalBottom.Play("PlayerRunB_Left");
                break;

            case (3, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Left):
                skeletalTop.Play("Earth_T_R_3");
                skeletalBottom.Play("PlayerRunB_Left");
                break;
            case (3, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Left):
                skeletalTop.Play("Earth_T_L_3");
                skeletalBottom.Play("PlayerRunB_Left");
                break;
        }
    }

    void SetAnimBottom(EntityMovement move, LookDirection dir, EntityState state, Elements element, Dashing dashing) {
        switch(move, dir, state, element, dashing) {
            //Idle
            case (EntityMovement.Idle, LookDirection.Right, EntityState.None, _ , Dashing.No):
                skeletalBottom.Play("PlayerIdleB_Right");
                break;
            case (EntityMovement.Idle, LookDirection.Left, EntityState.None, _ , Dashing.No):
                skeletalBottom.Play("PlayerIdleB_Left");
                break;

            //Strafing
            case (EntityMovement.Strafing, LookDirection.Right, EntityState.None, _ , Dashing.No):
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (EntityMovement.Strafing, LookDirection.Left, EntityState.None, _ , Dashing.No):
                skeletalBottom.Play("PlayerRunB_Left");
                break;

            //Dashing
            case (EntityMovement.Strafing, LookDirection.Right, EntityState.None, _ , Dashing.Yes):
                skeletalBottom.Play("PlayerDashB_Right");
                break;
            case (EntityMovement.Strafing, LookDirection.Left, EntityState.None, _ , Dashing.Yes):
                skeletalBottom.Play("PlayerDashB_Left");
                break;

        }
    }

    void SetAnimTop(EntityMovement move, LookDirection dir, EntityState state, Elements element, Dashing dashing) {
        switch(state, move, dir, element, dashing) {
            //None | Idle | None
            case (EntityState.None, EntityMovement.Idle, LookDirection.Right, _ , Dashing.No):
                skeletalTop.Play("PlayerIdleT_Right");
                break;
            case (EntityState.None, EntityMovement.Idle, LookDirection.Left, _ , Dashing.No):
                skeletalTop.Play("PlayerIdleT_Left");
                break;

            //None | Strafing
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Right, _ , Dashing.No):
                skeletalTop.Play("PlayerRunT_Right");
                break;
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Left, _ , Dashing.No):
                skeletalTop.Play("PlayerRunT_Left");
                break;

            //None | Dashing
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Right, _ , Dashing.Yes):
                skeletalTop.Play("PlayerDashT_Right");
                break;
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Left, _ , Dashing.Yes):
                skeletalTop.Play("PlayerDashT_Left");
                break;    
        }
    }
}
