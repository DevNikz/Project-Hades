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
    [SerializeReference] private Elements selectedElement;

    void Start() {
        skeletalTop = transform.Find("SpriteT").GetComponent<Animator>();
        skeletalBottom = transform.Find("SpriteB").GetComponent<Animator>();
    }

    public void SetMovement(EntityMovement value) { entityMovement = value; }
    public void SetState(EntityState value) { entityState = value; }
    public void SetDirection(LookDirection value) { entityDirection = value; }
    public void SetElements(Elements value) { elements = value; }
    public void SetSelectedElements(Elements value) { selectedElement = value; }

    void Update() {
        SetAnimBottom(entityMovement, entityDirection, entityState, elements, PlayerController.Instance.IsDashing(), PlayerController.Instance.IsHurt());
        SetAnimTop(entityMovement, entityDirection, entityState, elements, PlayerController.Instance.IsDashing(), PlayerController.Instance.IsHurt());
        UpdateAnimation(selectedElement);
    }

    void UpdateAnimation(Elements elements) {
        switch(elements) {
            case Elements.Earth:
                UpdateEarthAnimation();
                break;
            case Elements.Fire:
                UpdateFireAnimation();
                break;
        }
    }

    public void PlayHurt() {
        switch(entityDirection, entityState) {
            case (LookDirection.Left, EntityState.None):
                skeletalBottom.Play("PlayerHurtB_Left");
                skeletalTop.Play("PlayerHurtT_Left");
                break;
            case (LookDirection.Right, EntityState.None):
                skeletalBottom.Play("PlayerHurtB_Right");
                skeletalTop.Play("PlayerHurtT_Right");
                break;
        }
    }

    void UpdateEarthAnimation() {
        //Right
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_R_1")) {
            GetComponent<Combat>().EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_R_2")) {
            GetComponent<Combat>().EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_R_3")) {
            GetComponent<Combat>().EndCombo();
        }

        //Left
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_1")) {
            GetComponent<Combat>().EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_2")) {
            GetComponent<Combat>().EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_3")) {
            GetComponent<Combat>().EndCombo();
        }
    }

    void UpdateFireAnimation() {
        //Right
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_R_1")) {
            GetComponent<Combat>().EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_R_2")) {
            GetComponent<Combat>().EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_R_3")) {
            GetComponent<Combat>().EndCombo();
        }

        //Left
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_1")) {
            GetComponent<Combat>().EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_2")) {
            GetComponent<Combat>().EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_3")) {
            GetComponent<Combat>().EndCombo();
        }
    }

    public void PlayAnimation(int counter, AttackDirection dir, Elements element) {
        switch(counter, dir, element, entityMovement, entityDirection) {
            //Earth
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

            //Fire
            //Idle
            case (1, AttackDirection.Right, Elements.Fire, EntityMovement.Idle, _): 
                skeletalTop.Play("Fire_T_R_1");
                skeletalBottom.Play("Fire_B_R_1");
                break;
            case (1, AttackDirection.Left, Elements.Fire, EntityMovement.Idle, _):
                skeletalTop.Play("Fire_T_L_1");
                skeletalBottom.Play("Fire_B_L_1"); 
                break;

            case (2, AttackDirection.Right, Elements.Fire, EntityMovement.Idle, _): 
                skeletalTop.Play("Fire_T_R_2");
                skeletalBottom.Play("Fire_B_R_2");
                break;
            case (2, AttackDirection.Left, Elements.Fire, EntityMovement.Idle, _):
                skeletalTop.Play("Fire_T_L_2");
                skeletalBottom.Play("Fire_B_L_2");
                break;

            case (3, AttackDirection.Right, Elements.Fire, EntityMovement.Idle, _):
                skeletalTop.Play("Fire_T_R_3");
                skeletalBottom.Play("Fire_B_R_3"); 
                break;
            case (3, AttackDirection.Left, Elements.Fire, EntityMovement.Idle, _):
                skeletalTop.Play("Fire_T_L_3");
                skeletalBottom.Play("Fire_B_L_3"); 
                break;

            //Strafing | Right
            case (1, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Right): 
                skeletalTop.Play("Fire_T_R_1");
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (1, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Right):
                skeletalTop.Play("Fire_T_L_1");
                skeletalBottom.Play("PlayerRunB_Right");
                break;

            case (2, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Right): 
                skeletalTop.Play("Fire_T_R_2");
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (2, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Right):
                skeletalTop.Play("Fire_T_L_2");
                skeletalBottom.Play("PlayerRunB_Right");
                break;

            case (3, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Right):
                skeletalTop.Play("Fire_T_R_3");
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (3, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Right):
                skeletalTop.Play("Fire_T_L_3");
                skeletalBottom.Play("PlayerRunB_Right");
                break;

            //Strafing | Left
            case (1, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Left): 
                skeletalTop.Play("Fire_T_R_1");
                skeletalBottom.Play("PlayerRunB_Left");
                break;
            case (1, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Left):
                skeletalTop.Play("Fire_T_L_1");
                skeletalBottom.Play("PlayerRunB_Left");
                break;

            case (2, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Left): 
                skeletalTop.Play("Fire_T_R_2");
                skeletalBottom.Play("PlayerRunB_Left");
                break;
            case (2, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Left):
                skeletalTop.Play("Fire_T_L_2");
                skeletalBottom.Play("PlayerRunB_Left");
                break;

            case (3, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Left):
                skeletalTop.Play("Fire_T_R_3");
                skeletalBottom.Play("PlayerRunB_Left");
                break;
            case (3, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Left):
                skeletalTop.Play("Fire_T_L_3");
                skeletalBottom.Play("PlayerRunB_Left");
                break;
        }
    }

    void SetAnimBottom(EntityMovement move, LookDirection dir, EntityState state, Elements element, bool dash, bool hurt) {
        switch(move, dir, state, element, dash, hurt) {
            //Idle
            case (EntityMovement.Idle, LookDirection.Right, EntityState.None, _ , false, false):
                skeletalBottom.Play("PlayerIdleB_Right");
                break;
            case (EntityMovement.Idle, LookDirection.Left, EntityState.None, _ , false, false):
                skeletalBottom.Play("PlayerIdleB_Left");
                break;

            //Strafing
            case (EntityMovement.Strafing, LookDirection.Right, EntityState.None, _ , false, false):
                skeletalBottom.Play("PlayerRunB_Right");
                break;
            case (EntityMovement.Strafing, LookDirection.Left, EntityState.None, _ , false, false):
                skeletalBottom.Play("PlayerRunB_Left");
                break;

            //Dashing
            case (EntityMovement.Strafing, LookDirection.Right, EntityState.None, _ , true, false):
                skeletalBottom.Play("PlayerDashB_Right");
                break;
            case (EntityMovement.Strafing, LookDirection.Left, EntityState.None, _ , true, false):
                skeletalBottom.Play("PlayerDashB_Left");
                break;

            //Hurt
            case (_, LookDirection.Right, EntityState.None, _ , _ , true):
                skeletalBottom.Play("PlayerHurtB_Right");
                break;
            case (_, LookDirection.Left, EntityState.None, _ , _ , true):
                skeletalBottom.Play("PlayerHurtB_Left");
                break;

        }
    }

    void SetAnimTop(EntityMovement move, LookDirection dir, EntityState state, Elements element, bool dash, bool hurt) {
        switch(state, move, dir, element, dash, hurt) {
            //None | Idle | None
            case (EntityState.None, EntityMovement.Idle, LookDirection.Right, _ , false, false):
                skeletalTop.Play("PlayerIdleT_Right");
                break;
            case (EntityState.None, EntityMovement.Idle, LookDirection.Left, _ , false, false):
                skeletalTop.Play("PlayerIdleT_Left");
                break;

            //None | Strafing
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Right, _ , false, false):
                skeletalTop.Play("PlayerRunT_Right");
                break;
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Left, _ , false, false):
                skeletalTop.Play("PlayerRunT_Left");
                break;

            //None | Dashing
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Right, _ , true, false):
                skeletalTop.Play("PlayerDashT_Right");
                break;
            case (EntityState.None, EntityMovement.Strafing, LookDirection.Left, _ , true, false):
                skeletalTop.Play("PlayerDashT_Left");
                break;    

            //None | Hurt
            case (EntityState.None, _, LookDirection.Right, _ , _, true):
                skeletalTop.Play("PlayerHurtT_Right");
                break;
            case (EntityState.None, _, LookDirection.Left, _ , _, true):
                skeletalTop.Play("PlayerHurtT_Left");
                break;    
        }
    }
}
