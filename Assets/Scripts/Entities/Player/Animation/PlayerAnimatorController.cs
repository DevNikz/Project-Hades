using Sirenix.OdinInspector;
using UnityEngine;
using System;

public class PlayerAnimatorController : MonoBehaviour
{
    // Reference to player skeletal mesh (Bottom and Top)
    // Reference to current player movement, direction, and attack states

    [TitleGroup("References", "General Animator References", Alignment = TitleAlignments.Centered)]
    public bool ShowReferences;
    [ShowIfGroup("ShowReferences")]
    [BoxGroup("ShowReferences/Ref")]

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private Animator animator;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private EntityMovement entityMovement;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private EntityState entityState;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private LookDirection entityDirection;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private int element;

    [BoxGroup("ShowReferences/Ref")]
    [SerializeReference] private Elements selectedElement;

    public float xScale;
    public Vector3 Scale;
    private int comboCount;

    void Start() {
        animator = transform.Find("Anims").GetComponent<Animator>();

        xScale = animator.gameObject.transform.localScale.x;
        Scale = animator.gameObject.transform.localScale;
    }

    public void SetMovement(EntityMovement value) { entityMovement = value; }
    public void SetState(EntityState value) { entityState = value; }
    public void SetDirection(LookDirection value) { entityDirection = value; }
    //public void SetElements(Elements value) { elements = value; }
    public void SetSelectedElements(Elements value) { selectedElement = value; }

    void Update() {
        if(LevelTrigger.HudCheck == false) {
            //SetAnimBottom(entityMovement, entityDirection, entityState, elements, PlayerController.Instance.IsDashing(), PlayerController.Instance.IsHurt());
            SetDir(entityDirection);
            SetAnim(entityMovement, entityState, PlayerController.Instance.IsDashing(), PlayerController.Instance.IsHurt());
            UpdateAnimation(selectedElement);
        }

        else {
            SetPause();
        }
    }

    void SetDir(LookDirection dir)
    {
        switch (dir)
        {
            case LookDirection.Left:
                xScale = Math.Abs(xScale) * -1;
                break;
            case LookDirection.Right:
                xScale = Math.Abs(xScale);
                break;
        }

        animator.gameObject.transform.localScale = new Vector3(xScale, Scale.y, Scale.z);
    }

    void SetPause() {
        animator.Play("Player_Idle");
    }

    void UpdateAnimation(Elements elements) {
        switch(elements) {
            case Elements.Earth:
                CheckEarthAnimation();
                UpdateEarthAnimation();
                break;
            case Elements.Fire:
                CheckFireAnimation();
                UpdateFireAnimation();
                break;
            case Elements.Water:
                CheckWaterAnimation();
                UpdateWaterAnimation();
                break;
            case Elements.Wind:
                CheckWindAnimation();
                UpdateWindAnimation();
                break;
        }
    }

    void CheckEarthAnimation() {
        var clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        var clipSpeed = animator.GetCurrentAnimatorStateInfo(0).speed;
        var normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        Debug.Log(normTime);

        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth1st")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.58f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth2nd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth3rd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.7f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }

        //Left
        /*if(animator.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_1")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.58f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_2")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_3")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.7f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }*/
    }

    void CheckFireAnimation() {
        var clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        var clipSpeed = animator.GetCurrentAnimatorStateInfo(0).speed;
        var normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire1st")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.58f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire2nd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire3rd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }

        //Left
        /*if(animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_1")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.58f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_2")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_3")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }*/
    }

    void CheckWaterAnimation() {
        var clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        var clipSpeed = animator.GetCurrentAnimatorStateInfo(0).speed;
        var normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water1st")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.58f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water2nd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water3rd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }

        //Left
        /*if(animator.GetCurrentAnimatorStateInfo(0).IsName("Water_T_L_1")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.58f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Water_T_L_2")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Water_T_L_3")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }*/
    }

    void CheckWindAnimation() {
        var clipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        var clipSpeed = animator.GetCurrentAnimatorStateInfo(0).speed;
        var normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind1st")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.58f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind2nd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind3rd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }

        //Left
        /*if(animator.GetCurrentAnimatorStateInfo(0).IsName("Wind_T_L_1")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.58f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Wind_T_L_2")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Wind_T_L_3")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(normTime >= 0.6f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }*/
    }

    void UpdateEarthAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth1st")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth2nd")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth3rd")) {
            GetComponent<Combat>().EndCombo();
        }

        //Left
        /*if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_1")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_2")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_3")) {
            GetComponent<Combat>().EndCombo();
        }*/
    }

    void UpdateFireAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire1st")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire2nd")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire3rd")) {
            GetComponent<Combat>().EndCombo();
        }

        //Left
        /*if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_1")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_2")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Fire_T_L_3")) {
            GetComponent<Combat>().EndCombo();
        }*/
    }

    void UpdateWaterAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water1st")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water2nd")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water3rd")) {
            GetComponent<Combat>().EndCombo();
        }

        //Left
        /*if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Water_T_L_1")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Water_T_L_2")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Water_T_L_3")) {
            GetComponent<Combat>().EndCombo();
        }*/
    }
    
    void UpdateWindAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind1st")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind2nd")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind3rd")) {
            GetComponent<Combat>().EndCombo();
        }

        //Left
        /*if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Wind_T_L_1")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Wind_T_L_2")) {
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Wind_T_L_3")) {
            GetComponent<Combat>().EndCombo();
        }*/
    }

    public void SetData(int counter, Elements element)
    {
        switch(element)
        {
            case Elements.Earth:
                this.element = 1;
                break;
            case Elements.Fire:
                this.element = 2;
                break;
            case Elements.Water:
                this.element = 3;
                break;
            case Elements.Wind:
                this.element = 4;
                break;
        }

        comboCount = counter;
    }

    void SetAnim(EntityMovement move, EntityState state, bool dash, bool hurt) {
        switch (state, move, hurt) {
            //None | Idle | None
            case (EntityState.None, EntityMovement.Idle, false):
                animator.SetBool("isMoving", false);
                break;

            //None | Strafing
            case (EntityState.None, EntityMovement.Strafing, false):
                animator.SetBool("isMoving", true);
                break;

            //None | Hurt
            case (EntityState.None, _, true):
                animator.Play("Player_Hurt");
                break;

            //Dead
            case (EntityState.Dead, _, false):
                animator.Play("PlayerDeathT");
                break;

            //Attacking
            case (EntityState.Attack, _, false):
                animator.SetInteger("ComboCount", comboCount);
                animator.SetInteger("Element", element);
                break;
        }

        if (dash) animator.SetBool("isDashing", dash); //animator.Play("Player_Dashing");
        else animator.SetBool("isDashing", dash);  //animator.Play("Player_Dashing");
    }

    public void ResetComboCount()
    {
        comboCount = 0;
    }

    /*void SetAnimBottom(EntityMovement move, LookDirection dir, EntityState state, Elements element, bool dash, bool hurt) {
       switch(move, dir, state, element, dash, hurt) {
           //Idle
           case (EntityMovement.Idle, LookDirection.Right, EntityState.None, _ , false, false):
               animator.Play("PlayerIdleB_Right");
               break;
           case (EntityMovement.Idle, LookDirection.Left, EntityState.None, _ , false, false):
               animator.Play("PlayerIdleB_Left");
               break;

           //Strafing
           case (EntityMovement.Strafing, LookDirection.Right, EntityState.None, _ , false, false):
               
               break;
           case (EntityMovement.Strafing, LookDirection.Left, EntityState.None, _ , false, false):
               
               break;

           //Dashing
           case (EntityMovement.Strafing, LookDirection.Right, EntityState.None, _ , true, false):
               animator.Play("PlayerDashB_Right");
               break;
           case (EntityMovement.Strafing, LookDirection.Left, EntityState.None, _ , true, false):
               animator.Play("PlayerDashB_Left");
               break;

           //Hurt
           case (_, LookDirection.Right, EntityState.None, _ , _ , true):
               animator.Play("PlayerHurtB_Right");
               break;
           case (_, LookDirection.Left, EntityState.None, _ , _ , true):
               animator.Play("PlayerHurtB_Left");
               break;

           case (_, LookDirection.Right, EntityState.Dead, _, _, false):
               animator.Play("Player_DeathB");
               break;
           case (_, LookDirection.Left, EntityState.Dead, _, _, false):
               animator.Play("Player_DeathB");
               break;

       }
   }*/

    /*switch(counter, dir, element, entityMovement, entityDirection) {
            //Earth
            //Idle
            case (1, AttackDirection.Right, Elements.Earth, EntityMovement.Idle, _): 
                break;
            /*case (1, AttackDirection.Left, Elements.Earth, EntityMovement.Idle, _):
                animator.Play("Earth_T_L_1");
                animator.Play("Earth_B_L_1"); 
                break;

            case (2, AttackDirection.Right, Elements.Earth, EntityMovement.Idle, _): 
                animator.Play("Player_Earth2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Earth, EntityMovement.Idle, _):
                animator.Play("Earth_T_L_2");
                animator.Play("Earth_B_L_2");
                break;

            case (3, AttackDirection.Right, Elements.Earth, EntityMovement.Idle, _):
                animator.Play("Player_Earth3rd");
                break;
            /*case (3, AttackDirection.Left, Elements.Earth, EntityMovement.Idle, _):
                animator.Play("Earth_T_L_3");
                animator.Play("Earth_B_L_3"); 
                break;

            //Strafing | Right
            case (1, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Right): 
                animator.Play("Player_Earth1st");
                break;
            /*case (1, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Earth_T_L_1");
                
                break;

            case (2, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Right): 
                animator.Play("Player_Earth2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Earth_T_L_2");
                
                break;

            case (3, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Player_Earth3rd");
                break;
            /*case (3, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Earth_T_L_3");
                
                break;

            //Strafing | Left
            case (1, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Left): 
                animator.Play("Player_Earth1st");
                break;
            /*case (1, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Earth_T_L_1");
                
                break;

            case (2, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Left): 
                animator.Play("Player_Earth2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Earth_T_L_2");
                
                break;

            case (3, AttackDirection.Right, Elements.Earth, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Player_Earth3rd");
                break;
            /*case (3, AttackDirection.Left, Elements.Earth, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Earth_T_L_3");
                
                break;

            //Fire
            //Idle
            case (1, AttackDirection.Right, Elements.Fire, EntityMovement.Idle, _): 
                animator.Play("Player_Fire1st");
                break;
            /*case (1, AttackDirection.Left, Elements.Fire, EntityMovement.Idle, _):
                animator.Play("Fire_T_L_1");
                animator.Play("Fire_B_L_1"); 
                break;

            case (2, AttackDirection.Right, Elements.Fire, EntityMovement.Idle, _): 
                animator.Play("Player_Fire2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Fire, EntityMovement.Idle, _):
                animator.Play("Fire_T_L_2");
                animator.Play("Fire_B_L_2");
                break;

            case (3, AttackDirection.Right, Elements.Fire, EntityMovement.Idle, _):
                animator.Play("Player_Fire3rd");
                break;
            /*case (3, AttackDirection.Left, Elements.Fire, EntityMovement.Idle, _):
                animator.Play("Fire_T_L_3");
                animator.Play("Fire_B_L_3"); 
                break;

            //Strafing | Right
            case (1, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Right): 
                animator.Play("Player_Fire1st");
                break;
            /*case (1, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Fire_T_L_1");
                
                break;

            case (2, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Right): 
                animator.Play("Player_Fire2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Fire_T_L_2");
                
                break;

            case (3, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Player_Fire3rd");
                break;
            /*case (3, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Fire_T_L_3");
                
                break;

            //Strafing | Left
            case (1, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Left): 
                animator.Play("Player_Fire1st");
                break;
            /*case (1, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Fire_T_L_1");
                
                break;

            case (2, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Left): 
                animator.Play("Player_Fire2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Fire_T_L_2");
                
                break;

            case (3, AttackDirection.Right, Elements.Fire, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Player_Fire3rd");
                break;
            /*case (3, AttackDirection.Left, Elements.Fire, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Fire_T_L_3");
                
                break;
            
            //Water
            //Idle
            case (1, AttackDirection.Right, Elements.Water, EntityMovement.Idle, _): 
                animator.Play("Player_Water1st");
                break;
            /*case (1, AttackDirection.Left, Elements.Water, EntityMovement.Idle, _):
                animator.Play("Water_T_L_1");
                animator.Play("Water_B_L_1"); 
                break;

            case (2, AttackDirection.Right, Elements.Water, EntityMovement.Idle, _): 
                animator.Play("Player_Water2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Water, EntityMovement.Idle, _):
                animator.Play("Water_T_L_2");
                animator.Play("Water_B_L_2");
                break;

            case (3, AttackDirection.Right, Elements.Water, EntityMovement.Idle, _):
                animator.Play("Player_Water3rd");                break;
            /*case (3, AttackDirection.Left, Elements.Water, EntityMovement.Idle, _):
                animator.Play("Water_T_L_3");
                animator.Play("Water_B_L_3"); 
                break;

            //Strafing | Right
            case (1, AttackDirection.Right, Elements.Water, EntityMovement.Strafing, LookDirection.Right): 
                animator.Play("Player_Water1st");
                break;
            /*case (1, AttackDirection.Left, Elements.Water, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Water_T_L_1");
                
                break;

            case (2, AttackDirection.Right, Elements.Water, EntityMovement.Strafing, LookDirection.Right): 
                animator.Play("Player_Water2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Water, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Water_T_L_2");
                
                break;

            case (3, AttackDirection.Right, Elements.Water, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Player_Water3rd");
                break;
            /*case (3, AttackDirection.Left, Elements.Water, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Water_T_L_3");
                
                break;

            //Strafing | Left
            case (1, AttackDirection.Right, Elements.Water, EntityMovement.Strafing, LookDirection.Left): 
                animator.Play("Player_Water1st");
                break;
            /*case (1, AttackDirection.Left, Elements.Water, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Water_T_L_1");
                
                break;

            case (2, AttackDirection.Right, Elements.Water, EntityMovement.Strafing, LookDirection.Left): 
                animator.Play("Player_Water2nd");
                break;
            /*case (2, AttackDirection.Left, Elements.Water, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Water_T_L_2");
                
                break;

            case (3, AttackDirection.Right, Elements.Water, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Player_Water3rd");
                break;
            /*case (3, AttackDirection.Left, Elements.Water, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Water_T_L_3");
                
                break;

            //Wind
            //Idle
            case (1, AttackDirection.Right, Elements.Wind, EntityMovement.Idle, _): 
                animator.Play("Player_Wind1st");
                
                break;
            /*case (1, AttackDirection.Left, Elements.Wind, EntityMovement.Idle, _):
                animator.Play("Wind_T_L_1");
                animator.Play("Wind_B_L_1"); 
                break;

            case (2, AttackDirection.Right, Elements.Wind, EntityMovement.Idle, _): 
                animator.Play("Player_Wind2nd");
                
                break;
            /*case (2, AttackDirection.Left, Elements.Wind, EntityMovement.Idle, _):
                animator.Play("Wind_T_L_2");
                animator.Play("Wind_B_L_2");
                break;

            case (3, AttackDirection.Right, Elements.Wind, EntityMovement.Idle, _):
                animator.Play("Player_Wind3rd");

                break;
            /*case (3, AttackDirection.Left, Elements.Wind, EntityMovement.Idle, _):
                animator.Play("Wind_T_L_3");
                animator.Play("Wind_B_L_3"); 
                break;

            //Strafing | Right
            case (1, AttackDirection.Right, Elements.Wind, EntityMovement.Strafing, LookDirection.Right): 
                animator.Play("Player_Wind1st");
                
                break;
            /*case (1, AttackDirection.Left, Elements.Wind, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Wind_T_L_1");
                
                break;

            case (2, AttackDirection.Right, Elements.Wind, EntityMovement.Strafing, LookDirection.Right): 
                animator.Play("Player_Wind2nd");
                
                break;
            /*case (2, AttackDirection.Left, Elements.Wind, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Wind_T_L_2");
                
                break;

            case (3, AttackDirection.Right, Elements.Wind, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Player_Wind3rd");
                
                break;
            /*case (3, AttackDirection.Left, Elements.Wind, EntityMovement.Strafing, LookDirection.Right):
                animator.Play("Wind_T_L_3");
                
                break;

            //Strafing | Left
            case (1, AttackDirection.Right, Elements.Wind, EntityMovement.Strafing, LookDirection.Left): 
                animator.Play("Player_Wind1st");
                
                break;
            /*case (1, AttackDirection.Left, Elements.Wind, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Wind_T_L_1");
                
                break;

            case (2, AttackDirection.Right, Elements.Wind, EntityMovement.Strafing, LookDirection.Left): 
                animator.Play("Player_Wind2nd");
                
                break;
            /*case (2, AttackDirection.Left, Elements.Wind, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Wind_T_L_2");
                
                break;

            case (3, AttackDirection.Right, Elements.Wind, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Player_Wind3rd");
                
                break;
            /*case (3, AttackDirection.Left, Elements.Wind, EntityMovement.Strafing, LookDirection.Left):
                animator.Play("Wind_T_L_3");
                
                break;
        }
        */
}
