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
    [SerializeField] private bool debug;

    //References
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
    [ReadOnly] [SerializeReference] protected Animator skeletalBottom;

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
    [SerializeField] public TextMeshProUGUI fireChargeText;

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
        skeletalTop = transform.Find("SpriteT").GetComponent<Animator>();
        skeletalBottom = transform.Find("SpriteB").GetComponent<Animator>();
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        hitBox = skeletalTop.GetComponent<HitboxCall>();


        //Rather than finding it in scene, reference it in the scriptables
        hitBoxBasic = pointerUI.transform.Find("Melee").gameObject;
        hitboxLunge = pointerUI.transform.Find("Lunge").gameObject;
        hitboxDetain = pointerUI.transform.Find("Detain").gameObject;
        hitBoxBasic.SetActive(false);
        hitboxLunge.SetActive(false);
        hitboxDetain.SetActive(false);

        detainCooldown = 5.0f;
    }

    void OnEnable() {
        baseHealthDamage = combat.healthDamage;
        modHealthDamage = combat.healthDamage;

        basePoiseDamage = combat.poiseDamage;
        modPoiseDamage = basePoiseDamage;

        hitBoxBasic.SetActive(false);
        hitboxLunge.SetActive(false);
        hitboxDetain.SetActive(false);
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        tempTimer = 0;
        comboCounter = 0;
        detainTimer = 0;
        Debug.Log("Combat Enabled!");
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.StateHandler);
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
        UpdatePointer();
        UpdateTimer();
        UpdateAttackDirection();
        SwitchWeapon();

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

    void StateHandler(Parameters parameters) {
        leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);
        detainPress = parameters.GetBoolExtra(DETAIN, false);

        //Of course it will cause an inf. loop if I set it to a while loop. Dumbass.
        //Debug.Log(MenuScript.weaponWheelCheck);
        if(IsMouseOverGameWindow && MenuScript.weaponWheelCheck == false) {
            switch(leftClick, detainPress) {
                case (true, false):
                    switch(selectedElement) {
                        case Elements.Earth:
                            InitAttack(Elements.Earth);
                            break;
                        case Elements.Fire:
                            InitAttack(Elements.Fire);
                            break;
                        case Elements.Water:
                            InitAttack(Elements.Water);
                            break;
                        case Elements.Wind:
                            InitAttack(Elements.Wind);
                            break;
                    }
                    break;
                case (false, true):
                    if(!playerSeen) InitDetain();
                    break;
            }
        }
        animatorController.PlayAnimation(comboCounter, tempDirection, elements);
    }

    void InitAttack(Elements selectedElement) {
        if(Time.time - lastComboEnd > 0.5f & comboCounter <= 3) {
            if(Time.time - lastClickedTime >= 0.2f) {
                tempDirection = attackDirection;
                deltaState = entityState;
                deltaDir = entityDir;

                entityState = EntityState.Attack;
                elements = selectedElement;
                
                comboCounter++;
                lastClickedTime = Time.time;

                gameObject.GetComponent<PlayerController>().UpdateMana(false);

                if (comboCounter == 1) {
                    InitHitBox(hitBoxBasic, "PlayerMelee", debug);
                }

                else if(comboCounter == 2) {
                    InitHitBox(hitBoxBasic, "PlayerMelee", debug);
                }
                
                else if(comboCounter == 3) { 
                    InitHitBox(hitBoxBasic, "PlayerMeleeLarge", debug);
                }
            }
        }
    }

    void InitDetain() {
        if(detainTimer >= detainCooldown)
        {
            detainTimer = 0;
                
            comboCounter = 0;

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

    
    void InitHitBox(GameObject hitBoxRef, string attackTag, bool isDebug) {       
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

        //Init Stats
        if (hitboxLeft_Temp.CompareTag("Detain")){
            hitboxLeft_Temp.GetComponent<MeleeController>().SetHealthDamage(120);
            hitboxLeft_Temp.GetComponent<MeleeController>().SetStunDamage(modPoiseDamage);
        }
        else {
            hitboxLeft_Temp.GetComponent<MeleeController>().SetHealthDamage(modHealthDamage);
            hitboxLeft_Temp.GetComponent<MeleeController>().SetStunDamage(modPoiseDamage);
        }


        //Start Timer for hitbox (To mimic ticks)
        hitboxLeft_Temp.GetComponent<MeleeController>().StartTimer();
        
        //Set Pos of hitbox
        if(tempDirection == AttackDirection.Right) {
            hitboxLeft_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Right);
        }

        else {
            hitboxLeft_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Left);
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
            gameObject.GetComponent<PlayerController>().UpdateMana(true);
        }
    }

    public void SetHealthDamage(float value) {
        modHealthDamage = baseHealthDamage;
        modHealthDamage += value;
    }

    public void SetStunDamage(float value) {
        modPoiseDamage = basePoiseDamage;
        modPoiseDamage += value;
    }
}
