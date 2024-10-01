using Sirenix.OdinInspector;
using TMPro;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    //Basic Attack (Left Click)
    [PropertySpace] [TitleGroup("Properties", "General Combat Properties", TitleAlignments.Centered)]
    [AssetSelector]
    public PlayerAttackScriptable combat;

    [BoxGroup("Properties/Box", ShowLabel = false)]
    [ReadOnly, SerializeReference] private float lastClickedTime;
    
    [BoxGroup("Properties/Box", ShowLabel = false)]
    [ReadOnly, SerializeReference] private float lastComboEnd;

    [BoxGroup("Properties/Box", ShowLabel = false)]
    [ReadOnly, SerializeReference] private int comboCounter;
    
    //Alternate Attack(Right Click)
    [BoxGroup("Properties/Box", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected bool rightClick;

    //Timer
    [PropertySpace] [TitleGroup("Timer", "General Timer Settings", TitleAlignments.Centered)]
    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [ReadOnly] [SerializeField] private TimerState timerState;

    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [ReadOnly] [SerializeField] private TimerState timerFlickState;

    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [Range(0.1f, 10f)] public float comboTimer = 1;

    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [ReadOnly] public float tempTimer;

    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [ReadOnly] public float detainTimer;

    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [SerializeField] public float detainCooldown;

    [PropertySpace] [TitleGroup("References", "General References", TitleAlignments.Centered)] 

    [BoxGroup("References/Debug", ShowLabel = false)]
    [ReadOnly] public Vector3 tempPosition;

    [BoxGroup("References/Debug", ShowLabel = false)]
    [ReadOnly] public Vector3 tempVect;

    [BoxGroup("References/Debug", ShowLabel = false)]
    [ReadOnly] public float tempflicktime;

    [BoxGroup("References/Debug", ShowLabel = false)]
    [ReadOnly] public Vector3 tempPos;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] public PlayerAnimatorController animatorController;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] public GameObject hitBoxBasic;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] public GameObject hitboxLunge;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] public GameObject hitboxLeft_Temp;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] public GameObject hitboxLunge_Temp;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] public GameObject hitboxDetain;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] public GameObject hitboxDetain_Temp;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected GameObject pointerUI;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected GameObject attackUI;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected Slider attackUISlider;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected RectTransform attackUIEnd;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected Animator skeletalTop;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected AttackDirection attackDirection;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected AttackDirection tempDirection;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected EntityMovement entityMovement = EntityMovement.Idle;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected EntityState entityState;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected EntityDirection entityDir;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected EntityState deltaState;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected EntityDirection deltaDir;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [SerializeField] public TextMeshProUGUI fireChargeText;

    [PropertySpace] [TitleGroup("Miscallaneous", "[For Debug Purposes]", alignment: TitleAlignments.Split)]

    [BoxGroup("Miscallaneous/BasicAttack", ShowLabel = false)]
    [ReadOnly] public bool leftClick;

    [BoxGroup("Miscallaneous/BasicAttack", ShowLabel = false)]
    [ReadOnly] public Vector3 RStickInput;

    [BoxGroup("Miscallaneous/BasicAttack", ShowLabel = false)]
    [ReadOnly] public bool detainPress;

    [BoxGroup("Miscallaneous/BasicAttack", ShowLabel = false)]
    [ReadOnly] public bool playerSeen = false;

    [BoxGroup("Miscallaneous/BasicAttack", ShowLabel = false)]

    [BoxGroup("Miscallaneous/Pointer", ShowLabel = false)]
    [HideLabel] [ReadOnly] [SerializeReference] protected Vector3 tempVector;
    
    [BoxGroup("Miscallaneous/Pointer", ShowLabel = false)]
    [HideLabel] [ReadOnly] [SerializeReference] protected float angle;

    [BoxGroup("Miscallaneous/Pointer", ShowLabel = false)]
    [HideLabel] [ReadOnly] [SerializeReference] protected Quaternion rot;

    [BoxGroup("Miscallaneous/Pointer", ShowLabel = false)]
    [HideLabel] [ReadOnly] [SerializeReference] protected float rotX;

    [PropertySpace, TitleGroup("Elemental Charges", "Elements Properties", TitleAlignments.Centered)]
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int maxFireCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField] private int currentFireCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int maxWaterCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField] private int currentWaterCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int maxEarthCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField] private int currentEarthCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int maxWindCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField] private int currentWindCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int elementChargeDecrement;

    //Broadcaster
    public const string LEFT_CLICK = "LEFT_CLICK";
    public const string RIGHT_CLICK = "RIGHT_CLICK";
    public const string DETAIN = "DETAIN";
    public const string HIDDEN = "HIDDEN";
    public const string ENEMY_KILLED = "ENEMY_KILLED";

    void Awake() {
        //Reference
        animatorController = this.GetComponent<PlayerAnimatorController>();
        combat = Resources.Load<PlayerAttackScriptable>("Player/Combat/PlayerAttack");
        pointerUI = transform.Find("Pointer").gameObject;
        attackUI = transform.Find("AttackUI").gameObject;
        attackUISlider = attackUI.transform.Find("Border").transform.Find("StartBase").transform.Find("Slider").GetComponent<Slider>();
        attackUIEnd = attackUI.transform.Find("Border").transform.Find("EndBase").transform.Find("End").GetComponent<RectTransform>();
        skeletalTop = transform.Find("SpriteT").GetComponent<Animator>();
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        timerState = TimerState.None;

        //Rather than finding it in scene, reference it in the scriptables
        hitBoxBasic = pointerUI.transform.Find("Melee").gameObject;
        hitboxLunge = pointerUI.transform.Find("Lunge").gameObject;
        hitboxDetain = pointerUI.transform.Find("Detain").gameObject;
        hitBoxBasic.SetActive(false);
        hitboxLunge.SetActive(false);
        hitboxDetain.SetActive(false);

        //fireChargeText.text = "Current Fire Charge: " + currentFireCharge.ToString();
        detainCooldown = 5.0f;

        fireChargeText = GameObject.Find("/GeneralObjects/UI/FireChargeText").GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() {
        hitBoxBasic.SetActive(false);
        hitboxLunge.SetActive(false);
        hitboxDetain.SetActive(false);
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        timerState = TimerState.None;
        tempTimer = 0;
        comboCounter = 0;
        detainTimer = 0;
        Debug.Log("Combat Enabled!");
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.DETAIN_PRESS, this.DetainAttackState);
        EventBroadcaster.Instance.AddObserver(EventNames.Combat.PLAYER_SEEN, this.SetPlayerSeen);
        EventBroadcaster.Instance.AddObserver(EventNames.Combat.ENEMY_KILLED, this.UpdateElementCharge);
    }

    void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.DETAIN_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Combat.PLAYER_SEEN);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Combat.ENEMY_KILLED);
    }

    void Update() {
        UpdatePointer();
        UpdateTimer();
        UpdateAttackDirection();
        SwitchWeapon();
        UpdateUI();

        animatorController.SetState(entityState);
        
        //Temp
        tempPos = new Vector3(tempVector.x, this.transform.position.y, tempVector.y).normalized;

        if (entityState == EntityState.Attack && MenuScript.LastSelection == 3)
        {
            UpdateAnimation();
        }
        // else if (PlayerData.isAttacking && MenuScript.LastSelection == 2)
        // {
        //     UpdateFireAnimation();
        // }
        else if (entityState == EntityState.Attack)
        {
            UpdateAnimation();
        }
    }

    void UpdateUI() {
        fireChargeText.text = "Current Fire Charge: " + currentFireCharge.ToString();
    }

    void UpdateAttackDirection() {
        if(angle >= 0 && angle <= 90) attackDirection = AttackDirection.Right;
        else if(angle <= 0 && angle >= -90) attackDirection = AttackDirection.Right;
        else attackDirection = AttackDirection.Left;
    }

    void UpdateAnimation() {
        //Right
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_R_1")) {
            EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_R_2")) {
            EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_R_3")) {
            EndCombo();
        }

        //Left
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_1")) {
            EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_2")) {
            EndCombo();
        }
        if(skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_3")) {
            EndCombo();
        }
    }

    // void UpdateFireAnimation()
    // {
    //     temptime = attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;    
    //     //Right
    //     if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR1"))
    //     {
    //         counter = 0;
    //         timerState = TimerState.Stop;
    //     }
    //     if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR2"))
    //     {
    //         counter = 0;
    //         timerState = TimerState.Stop;
    //     }
    //     if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR3"))
    //     {
    //         counter = 0;
    //         timerState = TimerState.Stop;
    //     }


    //     //Left
    //     if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL1"))
    //     {
    //         counter = 0;
    //         timerState = TimerState.Stop;
    //     }
    //     if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL2"))
    //     {
    //         counter = 0;
    //         timerState = TimerState.Stop;
    //     }
    //     if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL3"))
    //     {
    //         counter = 0;
    //         timerState = TimerState.Stop;
    //     }
    // }

    void SwitchWeapon()
    {
        int lastWeapon = MenuScript.LastSelection;

        switch (lastWeapon)
        {
            //Earth can't be loaded as of the moment.
            //LastSelection returns 0 by default in MenuScript, but Earth's position in wheel is also 0.
            //Declaring LastSelection as -1 breaks the movement.
            //Earth image is set to index 0 in weapon wheel ui, could prolly add some kind of filler image to move to 1?

            //Note for future ref: Dmg and stun calc is done in EnemyController

            case 1: //Earth
                EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
                EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState); //Change to earth(basic?) eventually
                break;
            
            case 2: //Fire
                // EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
                // EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.FireAttack);
                break;

            case 3: //Water
                EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
                EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState); //Change to water
                break;

            case 4: //Wind
                EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
                EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState); //Change to wind eventually
                break;

            default:
                EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
                EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState);
                break;

            //Make more animations in the future, will figure out what to do with default case later.
        }
    }

    void EndCombo() {
        Debug.Log("Combo End");
        comboCounter = 0;
        lastComboEnd = Time.time;
        entityState = EntityState.None;
    }

    //Basic Attack
    void BasicAttackState(Parameters parameters) {
        //Gonna do queued inputs instead lol
        leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);

        if(leftClick && IsMouseOverGameWindow) {
            tempDirection = attackDirection;
            if(Time.time - lastComboEnd > 0.5f & comboCounter <= 3) {
                if(Time.time - lastClickedTime >= 0.2f) {

                    deltaState = entityState;
                    deltaDir = entityDir;

                    entityState = EntityState.Attack;
                    
                    comboCounter++;
                    lastClickedTime = Time.time;

                    if (comboCounter > 3) {
                        comboCounter = 1;
                    }

                    if(comboCounter == 1) {
                        Debug.Log("Combo 1!");
                        InitHitBox(hitBoxBasic, new Vector3(1.8f, 3f, 1.2f), "PlayerMelee");
                    }

                    else if(comboCounter == 2) {
                        Debug.Log("Combo 2!");
                        InitHitBox(hitBoxBasic, new Vector3(1.8f, 3f, 1.2f), "PlayerMelee");
                    }
                    
                    else if(comboCounter == 3) {
                        Debug.Log("Combo 3!");
                        InitHitBox(hitBoxBasic, new Vector3(2.615041f, 5.071505f, 1.2f), "PlayerMelee");
                    }
                }
            }
        }

        SwitchAnimation();
    }

    // void FireAttack(Parameters parameters)
    // {
    //     leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);

    //     if (leftClick && IsMouseOverGameWindow)
    //     {
    //         // PlayerData.isAttacking = true;
    //         // entityState = PlayerData.entityState;
    //         // etn = PlayerData.entityDirection;

    //         timerState = TimerState.Start;
    //         counter++;

    //         tempDirection = attackDirection;

    //     }
    //     // else
    //     // {
    //     //     if (leftClick)
    //     //     {
    //     //         PlayerData.isAttacking = true;
    //     //         deltaState = PlayerData.entityState;
    //     //         deltaDir = PlayerData.entityDirection;

    //     //         timerState = TimerState.Start;
    //     //         counter++;

    //     //         tempDirection = attackDirection;
    //     //     }
    //     // }

    //     if (leftClick && counter == 1)
    //     {
    //         InitHitBoxLeft();
    //         if (currentFireCharge > 0)
    //         {
    //             currentFireCharge = currentFireCharge - elementChargeDecrement;
    //             fireChargeText.text = "Current Fire Charge: " + currentFireCharge.ToString();
    //         }

    //     }

    //     if (leftClick && counter == 2)
    //     {
    //         InitHitBoxLeft();
    //         if (currentFireCharge > 0)
    //         {
    //             currentFireCharge = currentFireCharge - elementChargeDecrement;
    //             fireChargeText.text = "Current Fire Charge: " + currentFireCharge.ToString();
    //         }

    //     }

    //     if (leftClick && counter == 3)
    //     {
    //         tempPosition = GetTempPosition();
    //         tempVect = GetTempVector();
    //         InitHitBoxLunge();

    //         if (currentFireCharge > 0)
    //         {
    //             currentFireCharge = currentFireCharge - elementChargeDecrement;

    //             fireChargeText.text = "Current Fire Charge: " + currentFireCharge.ToString();
    //         }

    //     }

    //     counter = Mathf.Clamp(counter, 0, 3);

    //     // SwitchFireAnimation();
    //     // UpdateLunge();
    // }

    //DetainAttack (copy of basic attack for now)
    void DetainAttackState(Parameters parameters)
    {
        detainPress = parameters.GetBoolExtra(DETAIN, false);
        

        if(detainPress && !this.playerSeen)
        {
            if(this.detainTimer >= this.detainCooldown)
            {
                this.detainTimer = 0;

                    // PlayerData.isAttacking = true;
                    // deltaState = PlayerData.entityState;
                    // deltaDir = PlayerData.entityDirection;
                    
                timerState = TimerState.Start;
                comboCounter = 1;

                tempDirection = attackDirection;

                InitHitBox(hitboxDetain, new Vector3(1.8f, 3f, 1.2f), "PlayerMelee");
            }

            else
            {
                Debug.Log("Cannot Detain: On Cooldown!");
            }
        }

        else if(detainPress && this.playerSeen)
        {
            Debug.Log("Cannot Detain: Player is visible to enemies!");
        }

        comboCounter = Mathf.Clamp(comboCounter, 0, 3);

        // SwitchAnimation();
        // UpdateLunge();
    }

    void SwitchAnimation() {
        //1st Move
        //The other conditions are for the unanimated aspects. They're just here to prevent some jank while doing the demo.
        if(comboCounter == 1 && (MenuScript.LastSelection == 3 || MenuScript.LastSelection == 0 || MenuScript.LastSelection == 1 || MenuScript.LastSelection == 4) ) {
            if(tempDirection == AttackDirection.Right) skeletalTop.Play("Earth_T_R_1");
            else skeletalTop.Play("Earth_T_L_1");
        }

        //2nd Move
        if(comboCounter >= 2 && skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_R_1")) {
            if(tempDirection == AttackDirection.Right) skeletalTop.Play("Earth_T_R_2");
            else skeletalTop.Play("Earth_T_L_2");
        }

        if(comboCounter >= 2 && skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_1")) {
            if(tempDirection == AttackDirection.Right) skeletalTop.Play("Earth_T_R_2");
            else skeletalTop.Play("Earth_T_L_2");
            
        }

        //3rd Move
        if(comboCounter >= 3 && skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_R_2")) {
            if(tempDirection == AttackDirection.Right) skeletalTop.Play("Earth_T_R_3");
            else skeletalTop.Play("Earth_T_L_3");
        }

        if(comboCounter >= 3 && skeletalTop.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && skeletalTop.GetCurrentAnimatorStateInfo(0).IsName("Earth_T_L_2")) {
            if(tempDirection == AttackDirection.Right) skeletalTop.Play("Earth_T_R_3");
            else skeletalTop.Play("Earth_T_L_3");
        }
    }

    // void SwitchFireAnimation()
    // {
    //     //1st Move
    //     if (counter == 1 && MenuScript.LastSelection == 2)
    //     {
    //         //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f);
    //         if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR1");
    //         else attackAnimator.Play("FireAtkL1");

    //     }

    //     //2nd Move
    //     if (counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR1"))
    //     {
    //         //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
    //         if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR2");
    //         else attackAnimator.Play("FireAtkL2");
    //     }

    //     if (counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL1"))
    //     {
    //         //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
    //         if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR2");
    //         else attackAnimator.Play("FireAtkL2");

    //     }

    //     //3rd Move
    //     if (counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR2"))
    //     {
    //         tempflicktime = combat.flicktime;
    //         timerFlickState = TimerState.Start;

    //         if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR3");
    //         else attackAnimator.Play("FireAtkL3");
    //     }

    //     if (counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL2"))
    //     {
    //         tempflicktime = combat.flicktime;
    //         timerFlickState = TimerState.Start;

    //         if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR3");
    //         else attackAnimator.Play("FireAtkL3");
    //     }
    // }

    //THIS NEEDS UPDATING! Will definitely make one function for calling all kinds of hitbox
    void InitHitBox(GameObject hitBoxRef, Vector3 scale, string attackTag) {
        //Instantiate hitbox from selected attack type
        hitboxLeft_Temp = Instantiate(hitBoxRef, hitBoxRef.transform.position, pointerUI.transform.rotation);

        //To Fix scaling
        hitboxLeft_Temp.transform.localScale = scale;

        //Init tag based on attack type (i.e. PlayerMelee, etc)
        hitboxLeft_Temp.tag = attackTag;

        //Start Timer for hitbox (To mimic ticks)
        hitboxLeft_Temp.GetComponent<MeleeController>().StartTimer();
        
        //Set Pos of hitbox
        if(tempDirection == AttackDirection.Right) {
            hitboxLeft_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Right);
        }

        else {
            hitboxLeft_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Left);
        }

        //Activate it 
        hitboxLeft_Temp.SetActive(true);
    }

    void UpdateTimer() {
        //Hijacked for Detain Cooldown :)
        if(this.detainTimer < this.detainCooldown)
            this.detainTimer += Time.deltaTime;
    }


    //Pointer
    void UpdatePointer() {
        pointerUI.transform.position = new Vector3(this.transform.position.x, 0.05f, this.transform.position.z);
        toIsoRotation();
        rot = Quaternion.Euler(rotX, -angle-45, 0.0f);
        pointerUI.transform.rotation = rot;
    }

    void toIsoRotation() {
        tempVector = Camera.main.WorldToScreenPoint(pointerUI.transform.position);
        tempVector = Input.mousePosition - tempVector;
        angle = Mathf.Atan2(tempVector.y, tempVector.x) * Mathf.Rad2Deg;
    }

    bool IsMouseOverGameWindow
    {
        get
        {
            Vector3 mp = Input.mousePosition;
            return !( 0>mp.x || 0>mp.y || Screen.width<mp.x || Screen.height<mp.y );
        }
    }

    void SetPlayerSeen(Parameters param)
    {
        this.playerSeen = param.GetBoolExtra(HIDDEN, false);
    }

    void UpdateElementCharge(Parameters param)
    {
        bool enemyKilled = param.GetBoolExtra(ENEMY_KILLED, false);
        if(enemyKilled) Debug.Log(enemyKilled);

        int lastWeapon = MenuScript.LastSelection;

        if (enemyKilled && detainPress)
        {
            switch (lastWeapon)
            {
                case 1:
                    if (currentWindCharge < maxWindCharge)
                    {
                        Debug.Log("Wind charge update!");
                        currentWindCharge += 20;
                    }
                    break;
                case 2:
                    if (currentFireCharge < maxFireCharge)
                    {
                        Debug.Log("Fire charge update!");
                        currentFireCharge += 20;
                    }
                    break;
                case 3:
                    if (currentEarthCharge < maxEarthCharge)
                    {
                        Debug.Log("Earth charge update!");
                        currentEarthCharge += 20;
                    }
                    break;
                case 4:
                    if (currentWaterCharge < maxWaterCharge)
                    {
                        Debug.Log("Water charge update!");
                        currentWaterCharge += 20;
                    }
                    break;
            }
                
        }
    }

    public int GetCurrentFireCharge()
    {
        return currentFireCharge;
    }

    public int GetCurrentWaterCharge()
    {
        return currentFireCharge;
    }

    public int GetCurrentEarthCharge()
    {
        return currentFireCharge;
    }

    public int GetCurrentWindCharge()
    {
        return currentFireCharge;
    }
}
