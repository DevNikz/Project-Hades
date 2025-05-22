using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    //Basic Attack (Left Click)
    [PropertySpace] [TitleGroup("Properties (ReadOnly)", "", TitleAlignments.Centered)]
    [AssetSelector]

    //THIS IS FOR ALL THE DEFAULT BASE REFERENCES (AKA READONLY SHIT)
    public PlayerAttackScriptable combat;

    [PropertySpace, TitleGroup("Properties", "Player Combat Properties", TitleAlignments.Centered)]
    //player damage and stun

    [BoxGroup("Properties/0Box", ShowLabel = false)]
    [SerializeField] public float baseHealthDamage;

    [BoxGroup("Properties/0Box", ShowLabel = false)]
    [SerializeField] public float modHealthDamage;

    [BoxGroup("Properties/0Box", ShowLabel = false)]
    [SerializeField] public float basePoiseDamage;

    [BoxGroup("Properties/0Box", ShowLabel = false)]
    [SerializeField] public float modPoiseDamage;

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
    [Range(0.1f, 10f)] public float comboTimer = 1;

    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [ReadOnly] public float tempTimer;

    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [ReadOnly] public float detainTimer;

    [BoxGroup("Timer/TimerSettings", ShowLabel = false)]
    [SerializeField] public float detainCooldown;

    [PropertySpace] [TitleGroup("Debug", "Genreal Debug Stuffs", TitleAlignments.Centered)]
    [SerializeField] public bool debug;

    //References
    [PropertySpace] [TitleGroup("References", "General References", TitleAlignments.Centered)] 

    [BoxGroup("References/Debug")]
    [ReadOnly] public float AttackPressedTimer;

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
    [ReadOnly] [SerializeReference] protected Animator animator;

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
    [ReadOnly] [SerializeReference] protected Elements elements;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected Elements selectedElement;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected EntityState deltaState;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [ReadOnly] [SerializeReference] protected EntityDirection deltaDir;

    [BoxGroup("References/Ref", ShowLabel = false)]
    [SerializeReference] public HitboxCall hitBox;

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
    [ReadOnly] public bool leftClickAttacked;

    [BoxGroup("Miscallaneous/Pointer", ShowLabel = false)]
    [HideLabel] [ReadOnly] [SerializeReference] protected Vector3 tempVector;
    
    [BoxGroup("Miscallaneous/Pointer", ShowLabel = false)]
    [HideLabel] [ReadOnly] [SerializeReference] protected float angle;

    [BoxGroup("Miscallaneous/Pointer", ShowLabel = false)]
    [HideLabel] [ReadOnly] [SerializeReference] protected Quaternion rot;

    [BoxGroup("Miscallaneous/Pointer", ShowLabel = false)]
    [HideLabel] [ReadOnly] [SerializeReference] protected float rotX;

    

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
        pointerUI = transform.Find("AttackColliders").gameObject;
        attackUI = transform.Find("AttackUI").gameObject;
        attackUISlider = attackUI.transform.Find("Border").transform.Find("StartBase").transform.Find("Slider").GetComponent<Slider>();
        attackUIEnd = attackUI.transform.Find("Border").transform.Find("EndBase").transform.Find("End").GetComponent<RectTransform>();
        animator = transform.Find("Anims").GetComponent<Animator>();
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        hitBox = animator.GetComponent<HitboxCall>();


        //Rather than finding it in scene, reference it in the scriptables
        hitBoxBasic = pointerUI.transform.Find("Melee").gameObject;
        hitBoxBasic.SetActive(false);
        hitboxDetain = pointerUI.transform.Find("Detain").gameObject;
        hitboxDetain.SetActive(false);

        //Not used?
        /*hitboxLunge = pointerUI.transform.Find("Lunge").gameObject;
        /hitboxLunge.SetActive(false);
        */

        detainCooldown = 5.0f;
    }

    void OnEnable() {
        baseHealthDamage = combat.healthDamage;
        modHealthDamage = combat.healthDamage;

        basePoiseDamage = combat.poiseDamage;
        modPoiseDamage = basePoiseDamage;

        hitBoxBasic.SetActive(false);
        //hitboxLunge.SetActive(false);
        hitboxDetain.SetActive(false);
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        tempTimer = 0;
        comboCounter = 0;
        detainTimer = 0;
        Debug.Log("Combat Enabled!");
        //EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.StateHandler);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.DETAIN_PRESS, this.StateHandler);
        EventBroadcaster.Instance.AddObserver(EventNames.Combat.PLAYER_SEEN, this.SetPlayerSeen);
        EventBroadcaster.Instance.AddObserver(EventNames.Combat.ENEMY_KILLED, this.UpdateElementChargeOnKill);
    }

    void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.DETAIN_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Combat.PLAYER_SEEN);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Combat.ENEMY_KILLED);
    }

    void Update() {
        AttackPressedTimer -= Time.deltaTime;
        StateHandlerClick();
        UpdatePointer();
        UpdateTimer();
        UpdateAttackDirection();
        // SwitchWeapon();

        PlayerController.Instance.SetState(entityState);
        PlayerController.Instance.SetElements(elements);
        PlayerController.Instance.SetSelectedElements(selectedElement);
        
        //Temp
        tempPos = new Vector3(tempVector.x, this.transform.position.y, tempVector.y).normalized;
    }

    void UpdateAttackDirection() {
        if(angle >= 0 && angle <= 90) attackDirection = AttackDirection.Right;
        else if(angle <= 0 && angle >= -90) attackDirection = AttackDirection.Right;
        else attackDirection = AttackDirection.Left;
    }

    void StateHandlerClick() {
        bool pressed = Input.GetMouseButtonDown(0);

        //Of course it will cause an inf. loop if I set it to a while loop. Dumbass.
        //Debug.Log(MenuScript.weaponWheelCheck);
        if(IsMouseOverGameWindow && MenuScript.weaponWheelCheck == false && gameObject.tag == "Player" && LevelTrigger.HudCheck == false) {
            switch(pressed) {
                case true:
                    switch(PlayerStanceManager.Instance.SelectedStance) {
                        case EStance.Earth:
                            selectedElement = Elements.Earth;
                            InitAttack(Elements.Earth);
                            break;
                        case EStance.Fire:
                            selectedElement = Elements.Fire;
                            InitAttack(Elements.Fire);
                            break;
                        case EStance.Water:
                            selectedElement = Elements.Water;
                            InitAttack(Elements.Water);
                            break;
                        case EStance.Air:
                            selectedElement = Elements.Wind;
                            InitAttack(Elements.Wind);
                            break;
                    }
                    break;
            }
        }
        animatorController.PlayAttackAnim(comboCounter, elements);
    }
 
    void StateHandler(Parameters parameters) {
        leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);
        detainPress = parameters.GetBoolExtra(DETAIN, false);

        //Of course it will cause an inf. loop if I set it to a while loop. Dumbass.
        //Debug.Log(MenuScript.weaponWheelCheck);
        if(IsMouseOverGameWindow && MenuScript.weaponWheelCheck == false && gameObject.tag == "Player" && LevelTrigger.HudCheck == false) {
            switch(leftClick, detainPress) {
                case (true, false):
                    switch(PlayerStanceManager.Instance.SelectedStance) {
                        case EStance.Earth:
                            AttackPressedTimer = 2f;
                            InitAttack(Elements.Earth);
                            break;
                        case EStance.Fire:
                            InitAttack(Elements.Fire);
                            break;
                        case EStance.Water:
                            InitAttack(Elements.Water);
                            break;
                        case EStance.Air:
                            InitAttack(Elements.Wind);
                            break;
                    }
                    break;
                case (false, true):
                    if(!playerSeen) InitDetain();
                    break;
            }
        }
        animatorController.PlayAttackAnim(comboCounter, elements);
    }

    void InitAttack(Elements selectedElement) {
        switch(comboCounter) {
            case 0:
                leftClickAttacked = true;
                tempDirection = attackDirection;
                deltaState = entityState;
                deltaDir = entityDir;

                entityState = EntityState.Attack;
                elements = selectedElement;
                
                comboCounter++;
                lastClickedTime = Time.time;
                break;
            case 1:
            case 2:
            case 3:
                if(animator.GetFloat("AttackWindow.Open") > 0f) {
                    leftClickAttacked = true;
                    tempDirection = attackDirection;
                    deltaState = entityState;
                    deltaDir = entityDir;

                    entityState = EntityState.Attack;
                    elements = selectedElement;
                    
                    comboCounter++;
                    lastClickedTime = Time.time;
                }
                break;
            default:
                leftClickAttacked = true;
                tempDirection = attackDirection;
                deltaState = entityState;
                deltaDir = entityDir;

                entityState = EntityState.Attack;
                elements = selectedElement;
                
                comboCounter = 0;
                lastClickedTime = Time.time;
                break;
        }
        

        // if(Time.time - lastComboEnd > 0.5f & comboCounter <= 3) {
        //     if(Time.time - lastClickedTime >= 0.25f) {
        //         leftClickAttacked = true;
        //         tempDirection = attackDirection;
        //         deltaState = entityState;
        //         deltaDir = entityDir;

        //         entityState = EntityState.Attack;
        //         elements = selectedElement;
                
        //         comboCounter++;
        //         lastClickedTime = Time.time;

        //         PlayerController controller = gameObject.GetComponent<PlayerController>();
        //         controller.UpdateMana(-combat.manaCost);

        //         if(ItemManager.Instance.getAugment(AugmentType.Torrent_Gear).IsActive)
        //             controller.SetInvincibility(ItemManager.Instance.getAugment(AugmentType.Torrent_Gear).augmentPower);

        //         // if (comboCounter == 1) {
        //         //     InitHitBox(hitBoxBasic, "PlayerMelee", debug);
        //         // }

        //         // else if(comboCounter == 2) {
        //         //     InitHitBox(hitBoxBasic, "PlayerMelee", debug);
        //         // }
                
        //         // else if(comboCounter == 3) { 
        //         //     InitHitBox(hitBoxBasic, "PlayerMeleeLarge", debug);
        //         // }
        //     }
        // }
    }

    void InitDetain() {
        if(detainTimer >= detainCooldown)
        {
            detainTimer = 0;

            tempDirection = attackDirection;
            entityState = EntityState.Detain;

            InitHitBox(hitboxDetain, "Detain", debug);
        }
    }

    void SwitchWeapon()
    {
        int lastWeapon = MenuScript.LastSelection;
        switch (lastWeapon)
        {
            case 0: //Earth
                selectedElement = Elements.Earth;
                gameObject.GetComponent<PlayerController>().UpdateStyleIndicator("earth");
                break;
            
            case 1: //Water
                selectedElement = Elements.Water;
                gameObject.GetComponent<PlayerController>().UpdateStyleIndicator("water");
                break;

            case 2: //Wind
                selectedElement = Elements.Wind;
                gameObject.GetComponent<PlayerController>().UpdateStyleIndicator("wind");
                break;

            case 3: //Fire
                selectedElement = Elements.Fire;
                gameObject.GetComponent<PlayerController>().UpdateStyleIndicator("fire");
                break;

            default:
                selectedElement = Elements.None;
                //UpdateManaUI(selectedElement);
                break;
        }
    }

    public void EndCombo() {
        comboCounter = 0;
        lastComboEnd = Time.time;
        entityState = EntityState.None;
        elements = Elements.None;
    }

    
    public void InitHitBox(GameObject hitBoxRef, string attackTag, bool isDebug) {       
        // Call HitBox from animation instead | This will require queued inputs unfortunately
        // hitBox.hitBox = hitBoxRef;
        // hitBox.attackTag = attackTag;
        // hitBox.isDebug = isDebug;
        // hitBox.pointerRot = pointerUI.transform.rotation;
        // hitBox.healthDamage = modHealthDamage;
        // hitBox.poiseDamage = modPoiseDamage;
        // hitBox.direction = tempDirection;

        //Instantiate hitbox from selected attack type
        hitboxLeft_Temp = Instantiate(hitBoxRef, hitBoxRef.transform.position, pointerUI.transform.rotation);

        //Init tag based on attack type (i.e. PlayerMelee, etc)
        hitboxLeft_Temp.tag = attackTag;

        MeleeController tempMeleeController = hitboxLeft_Temp.GetComponent<MeleeController>();

        //Init Stats
        if (hitboxLeft_Temp.CompareTag("Detain")){
            tempMeleeController.SetHealthDamage(120);
            tempMeleeController.SetStunDamage(modPoiseDamage);
        }
        else {
            tempMeleeController.SetHealthDamage(modHealthDamage);
            tempMeleeController.SetStunDamage(modPoiseDamage);
        }


        //Start Timer for hitbox (To mimic ticks)
        tempMeleeController.StartTimer();
        
        //Set Pos of hitbox
        if(tempDirection == AttackDirection.Right) {
            tempMeleeController.SetAttackDirection(AttackDirection.Right);
        }

        else {
            tempMeleeController.SetAttackDirection(AttackDirection.Left);
        }

        // Set scale of hitbox
        if(ItemManager.Instance.getAugment(AugmentType.Wave_Gear).IsActive){
            Vector3 newScale = hitboxLeft_Temp.transform.localScale;
            newScale.x *= ItemManager.Instance.getAugment(AugmentType.Wave_Gear).augmentPower;
            newScale.y *= ItemManager.Instance.getAugment(AugmentType.Wave_Gear).augmentPower;
            newScale.z *= ItemManager.Instance.getAugment(AugmentType.Wave_Gear).augmentPower;
            hitboxLeft_Temp.transform.localScale = newScale;
        }

        //MeshRenderer | True = Debug | False = Release
        hitboxLeft_Temp.GetComponent<MeshRenderer>().enabled = isDebug;

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

    void UpdateElementChargeOnKill(Parameters param)
    {
        bool enemyKilled = param.GetBoolExtra(ENEMY_KILLED, false);
        if(enemyKilled) Debug.Log(enemyKilled);

        //int lastWeapon = MenuScript.LastSelection;

        if (enemyKilled && detainPress)
        {
            gameObject.GetComponent<PlayerController>().UpdateMana(combat.detainManaRecovery);
        }
    }

    public void SetHealthDamage(float value) {
        modHealthDamage = baseHealthDamage;
        modHealthDamage += (modHealthDamage * value);
    }

    public void SetStunDamage(float value) {
        modPoiseDamage = basePoiseDamage;
        modPoiseDamage += (modPoiseDamage * value);
    }
}
