using Sirenix.OdinInspector;
using TMPro;
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
        skeletalBottom = transform.Find("SpriteB").GetComponent<Animator>();
        rotX = pointerUI.transform.rotation.eulerAngles.x;


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
        tempTimer = 0;
        comboCounter = 0;
        detainTimer = 0;
        Debug.Log("Combat Enabled!");
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.StateHandler);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.DETAIN_PRESS, this.StateHandler);
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
        animatorController.SetElements(elements);
        animatorController.SetSelectedElements(selectedElement);
        
        //Temp
        tempPos = new Vector3(tempVector.x, this.transform.position.y, tempVector.y).normalized;
    }

    void UpdateUI() {
        fireChargeText.text = "Current Fire Charge: " + currentFireCharge.ToString();
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
        if(IsMouseOverGameWindow) {
            switch(leftClick, detainPress) {
                case (true, false):
                    switch(selectedElement) {
                        case Elements.Earth:
                            InitEarthAttack();
                            break;
                        case Elements.Fire:
                            break;
                        case Elements.Water:
                            break;
                        case Elements.Wind:
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

    void InitEarthAttack() {
        if(Time.time - lastComboEnd > 0.5f & comboCounter <= 3) {
            if(Time.time - lastClickedTime >= 0.2f) {
                tempDirection = attackDirection;
                deltaState = entityState;
                deltaDir = entityDir;

                entityState = EntityState.Attack;
                elements = Elements.Earth;
                
                comboCounter++;
                lastClickedTime = Time.time;

                if(comboCounter == 1) {
                    InitHitBox(hitBoxBasic, "PlayerMelee");
                }

                else if(comboCounter == 2) {
                    InitHitBox(hitBoxBasic, "PlayerMelee");
                }
                
                else if(comboCounter == 3) { 
                    InitHitBox(hitBoxBasic, "PlayerMeleeLarge");
                }       

                Debug.Log($"Counter: {comboCounter}");
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

            InitHitBox(hitboxDetain, "Detain");
        }
    }

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

            //Set Element instead. This will be used as ref to the statehandler

            case 0: //Earth
                selectedElement = Elements.Earth;
                break;
            
            case 1: //Fire
                selectedElement = Elements.Fire;
                break;

            case 2: //Water
                selectedElement = Elements.Water;
                break;

            case 3: //Wind
                selectedElement = Elements.Wind;
                break;

            default:
                selectedElement = Elements.None;
                break;
        }
    }

    public void EndCombo() {
        comboCounter = 0;
        lastComboEnd = Time.time;
        entityState = EntityState.None;
        elements = Elements.None;
    }

    
    void InitHitBox(GameObject hitBoxRef, string attackTag) {
        //Instantiate hitbox from selected attack type
        hitboxLeft_Temp = Instantiate(hitBoxRef, hitBoxRef.transform.position, pointerUI.transform.rotation);

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
