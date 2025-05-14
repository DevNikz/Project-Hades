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
            SetDir(entityDirection);
            PlayMovementAnim(entityMovement, entityState, PlayerController.Instance.IsDashing(), PlayerController.Instance.IsHurt());
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
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth1st")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth2nd")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth3rd")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
    }

    void CheckFireAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire1st")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire2nd")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire3rd")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
    }

    void CheckWaterAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water1st")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water2nd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water3rd")) {
            //Debug.Log($"Clip Length: {clipLength} | Clip Speed: {clipSpeed} | Time: {normTime} ");
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
    }

    void CheckWindAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind1st")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind2nd")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMelee", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind3rd")) {
            if(animator.GetFloat("AttackWindow.Open") > 0f && GetComponent<Combat>().leftClickAttacked == true) {
                GetComponent<Combat>().InitHitBox(GetComponent<Combat>().hitBoxBasic, "PlayerMeleeLarge", GetComponent<Combat>().debug);
                GetComponent<Combat>().leftClickAttacked = false;
            }
        }
    }

    void UpdateEarthAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth1st")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth2nd")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Earth3rd")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
    }

    void UpdateFireAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire1st")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire2nd")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Fire3rd")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
    }

    void UpdateWaterAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water1st")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water2nd")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Water3rd")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
    }
    
    void UpdateWindAnimation() {
        //Right
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind1st")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind2nd")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Wind3rd")) {
            animator.SetBool("isAttacking", false);
            GetComponent<Combat>().EndCombo();
        }
    }

    public void PlayAttackAnim(int counter, Elements element)
    {
        switch (counter, element)
        {
            //Earth
            case (1, Elements.Earth):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Earth1st");
                break;
            case (2, Elements.Earth):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Earth2nd");
                break;
            case (3, Elements.Earth):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Earth3rd");
                break;

            //Fire
            case (1, Elements.Fire):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Fire1st");
                break;
            case (2, Elements.Fire):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Fire2nd");
                break;
            case (3, Elements.Fire):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Fire3rd");
                break;

            //Water
            case (1, Elements.Water):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Water1st");
                break;
            case (2, Elements.Water):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Water2nd");
                break;
            case (3, Elements.Water):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Water3rd");
                break;

            //Wind
            case (1, Elements.Wind):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Wind1st");
                break;
            case (2, Elements.Wind):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Wind2nd");
                break;
            case (3, Elements.Wind):
                animator.SetBool("isAttacking", true);
                animator.Play("Player_Wind3rd");
                break;
        }
    }

    void PlayMovementAnim(EntityMovement move, EntityState state, bool dash, bool hurt)
    {
        switch (state, move, hurt)
        {
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
            case (EntityState.Dead, _, _):
                animator.Play("Player_DeathB");
                Debug.Log("Death Trigger");
                break;

            //Detaining
            case (EntityState.Detain, _, false):
                animator.SetBool("isDetaining", true);
                break;
        }

        if (dash) animator.SetBool("isDashing", dash);
        else animator.SetBool("isDashing", dash);

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Detain"))
        {
            animator.SetBool("isDetaining", false);
            GetComponent<Combat>().EndCombo();
        }
    }

    public float GetDeathAnimationLength()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Player_DeathB")
            {
                return clip.length;
            }
        }
        return 0f; 
    }


}
