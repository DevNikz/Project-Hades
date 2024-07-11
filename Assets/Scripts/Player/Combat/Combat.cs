using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    //Basic Attack (Left Click)
    [PropertySpace] [Title("Basic Attack")]
    [AssetSelector(Paths = "Assets/Data/Player/Combat")]
    public PlayerCombat combat;
    
    //Alternate Attack(Right Click)
    [Space] [Title("Alternate Attack")]
    [ReadOnly] [SerializeReference] protected bool rightClick;

    //Timer
    [Space] [Title("Timer Settings")]
    public bool ShowTimer;

    [ShowIfGroup("ShowTimer")]
    [BoxGroup("ShowTimer/TimerSettings")]
    [ReadOnly] [SerializeField] private TimerState timerState;

    [BoxGroup("ShowTimer/TimerSettings")]
    [ReadOnly] [SerializeField] private TimerState timerFlickState;

    [BoxGroup("ShowTimer/TimerSettings")]
    [ReadOnly] public float temptime;
    
    [Space] [Title("Temp(Debug)")] 
    public bool ShowDebug;

    [ShowIfGroup("ShowDebug")]
    [BoxGroup("ShowDebug/Debug")]
    [ReadOnly] public Vector3 tempPosition;

    [BoxGroup("ShowDebug/Debug")]
    [ReadOnly] public Vector3 tempVect;

    [BoxGroup("ShowDebug/Debug")]
    [ReadOnly] public float tempflicktime;

    [BoxGroup("ShowDebug/Debug")]
    [ReadOnly] public Vector3 tempPos;

    [Space] [TitleGroup("Miscallaneous", "[For Debug Purposes]", alignment: TitleAlignments.Split)]
    public bool BasicAttack;

    [ShowIfGroup("BasicAttack")]
    [BoxGroup("BasicAttack/BasicAttack")]
    [ReadOnly] public int counter = 0;

    [BoxGroup("BasicAttack/BasicAttack")]
    [ReadOnly] public bool leftClick;

    [BoxGroup("BasicAttack/BasicAttack")]
    [ReadOnly] public Vector3 RStickInput;

    [ReadOnly] public bool detainPress;

    [ReadOnly] public bool playerSeen = false;

    [Space] public bool Pointer;

    [ShowIfGroup("Pointer")]
    [BoxGroup("Pointer/Pointer")]
    [HideLabel] [ReadOnly] [SerializeReference] protected Vector3 tempVector;

    [BoxGroup("Pointer/Pointer")]
    [HideLabel] [ReadOnly] [SerializeReference] protected float angle;

    [BoxGroup("Pointer/Pointer")]
    [HideLabel] [ReadOnly] [SerializeReference] protected Quaternion rot;

    [BoxGroup("Pointer/Pointer")]
    [HideLabel] [ReadOnly] [SerializeReference] protected float rotX;

    [Space] public bool Reference;
    [ShowIfGroup("Reference")]

    [BoxGroup("Reference/References")]
    [ReadOnly] public GameObject hitboxLeft;

    [BoxGroup("Reference/References")]
    [ReadOnly] public GameObject hitboxLunge;

    [BoxGroup("Reference/References")]
    [ReadOnly] public GameObject hitboxLeft_Temp;

    [BoxGroup("Reference/References")]
    [ReadOnly] public GameObject hitboxLunge_Temp;

    [BoxGroup("Reference/References")]
    [ReadOnly] public GameObject hitboxDetain;

    [BoxGroup("Reference/References")]
    [ReadOnly] public GameObject hitboxDetain_Temp;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected GameObject pointerUI;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected GameObject attackUI;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected Slider attackUISlider;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected RectTransform attackUIEnd;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected Animator attackAnimator;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected AttackDirection attackDirection;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected AttackDirection tempDirection;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected EntityState deltaState;

    [BoxGroup("Reference/References")]
    [ReadOnly] [SerializeReference] protected EntityDirection deltaDir;

    //Broadcaster
    public const string LEFT_CLICK = "LEFT_CLICK";
    public const string RIGHT_CLICK = "RIGHT_CLICK";
    public const string RSTICK = "RSTICK";
    public const string DETAIN = "DETAIN";
    public const string HIDDEN = "HIDDEN";

    void Awake() {
        //Reference
        combat = Resources.Load<PlayerCombat>("Player/Combat/PlayerCombat");
        pointerUI = transform.Find("Pointer").gameObject;
        attackUI = transform.Find("AttackUI").gameObject;
        attackUISlider = attackUI.transform.Find("Border").transform.Find("StartBase").transform.Find("Slider").GetComponent<Slider>();
        attackUIEnd = attackUI.transform.Find("Border").transform.Find("EndBase").transform.Find("End").GetComponent<RectTransform>();
        attackAnimator = transform.Find("SpriteContainer").transform.Find("Sprite").GetComponent<Animator>();
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        timerState = TimerState.None;

        hitboxLeft = pointerUI.transform.Find("Melee").gameObject;
        hitboxLunge = pointerUI.transform.Find("Lunge").gameObject;
        hitboxDetain = pointerUI.transform.Find("Detain").gameObject;
        hitboxLeft.SetActive(false);
        hitboxLunge.SetActive(false);
        hitboxDetain.SetActive(false);

    }

    void OnEnable() {
        hitboxLeft.SetActive(false);
        hitboxLunge.SetActive(false);
        hitboxDetain.SetActive(false);
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        timerState = TimerState.None;
        temptime = 0;
        counter = 0;
        Debug.Log("Combat Enabled!");
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState);
        EventBroadcaster.Instance.AddObserver(EventNames.GamepadInput.RIGHT_STICK_INPUT, this.toIsoRotation_Gamepad);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.DETAIN_PRESS, this.DetainAttackState);
        EventBroadcaster.Instance.AddObserver(EventNames.Sight.PLAYER_SEEN, this.SetPlayerSeen);
    }

    void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.GamepadInput.RIGHT_STICK_INPUT);
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.DETAIN_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Sight.PLAYER_SEEN);
    }

    void Update() {
        UpdatePointer();
        UpdateTimer();
        UpdateAttackDirection();
        
        //Temp
        tempPos = new Vector3(tempVector.x, this.transform.position.y, tempVector.y).normalized;

        if(PlayerData.isAttacking) UpdateAnimation();
    }

    void UpdateAttackDirection() {
        if(angle >= 0 && angle <= 90) attackDirection = AttackDirection.Right;
        else if(angle <= 0 && angle >= -90) attackDirection = AttackDirection.Right;
        else attackDirection = AttackDirection.Left;
    }

    void UpdateAnimation() {
        temptime = attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //Right
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR1_New")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR2_New")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR3_New")) {
            counter = 0;
            timerState = TimerState.Stop;
        }


        //Left
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL1_New")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL2_New")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL3_New")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
    }

    //Basic Attack
    void BasicAttackState(Parameters parameters) {
        leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);
        
            if(Gamepad.all.Count == 0) {
                if(leftClick && IsMouseOverGameWindow) {
                    PlayerData.isAttacking = true;
                    deltaState = PlayerData.entityState;
                    deltaDir = PlayerData.entityDirection;

                    timerState = TimerState.Start;
                    counter++;

                    tempDirection = attackDirection;
                    
                }
            }
            else {
                if(leftClick) {
                    PlayerData.isAttacking = true;
                    deltaState = PlayerData.entityState;
                    deltaDir = PlayerData.entityDirection;

                    timerState = TimerState.Start;
                    counter++;

                    tempDirection = attackDirection;
                }
            }

            if(leftClick && counter == 1) {
                InitHitBoxLeft();
                LungePlayer();
            }

            if(leftClick && counter == 2) {
                InitHitBoxLeft();
                LungePlayer(combat.lungeForceMod);
            }
            
            if(leftClick && counter == 3) {
                tempPosition = GetTempPosition();
                tempVect = GetTempVector();
                InitHitBoxLunge();
            }

            counter = Mathf.Clamp(counter, 0, 3);

            SwitchAnimation();
            UpdateLunge();

    }

    //DetainAttack (copy of basic attack for now)
    void DetainAttackState(Parameters parameters)
    {
        detainPress = parameters.GetBoolExtra(DETAIN, false);
        

        if (Gamepad.all.Count == 0)
        {
            if (detainPress && !this.playerSeen)
            {
                PlayerData.isAttacking = true;
                deltaState = PlayerData.entityState;
                deltaDir = PlayerData.entityDirection;

                timerState = TimerState.Start;
                counter = 1;

                tempDirection = attackDirection;

                Debug.Log("Enemy Detained!");
            }
        }

        if(detainPress && !this.playerSeen)
        {
            InitHitBoxDetain();
        }

        counter = Mathf.Clamp(counter, 0, 3);

        SwitchAnimation();
        UpdateLunge();
    }

    void SwitchAnimation() {
        //1st Move
        if(counter == 1) {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR1_New");
            else attackAnimator.Play("BasicAtkL1_New");
        }

        //2nd Move
        if(counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR1_New")) {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR2_New");
            else attackAnimator.Play("BasicAtkL2_New");
        }

        if(counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL1_New")) {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR2_New");
            else attackAnimator.Play("BasicAtkL2_New");
            
        }

        //3rd Move
        if(counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR2_New")) {
            tempflicktime = combat.flicktime;
            timerFlickState = TimerState.Start;

            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR3_New");
            else attackAnimator.Play("BasicAtkL3_New");
        }

        if(counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL2_New")) {
            tempflicktime = combat.flicktime;
            timerFlickState = TimerState.Start;

            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR3_New");
            else attackAnimator.Play("BasicAtkL3_New");
        }
    }

    void UpdateLunge() {
        //3rd Move
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR3_New")) {
            if(hitboxLunge_Temp != null) {
                hitboxLunge_Temp.GetComponent<MeleeController>().StartTimer();
                hitboxLunge_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Right);
                hitboxLunge_Temp.SetActive(true);
            }
            
            LungePlayerAlt();
        }

        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL3_New")) {
            if(hitboxLunge_Temp != null) {
                hitboxLunge_Temp.GetComponent<MeleeController>().StartTimer();
                hitboxLunge_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Left);
                hitboxLunge_Temp.SetActive(true);
            }
            LungePlayerAlt();
        }
    }

    Vector3 GetTempPosition() {
        return tempPos;
    }

    Vector3 GetTempVector() {
        return tempVector;
    }

    void InitHitBoxLeft() {
        hitboxLeft_Temp = Instantiate(hitboxLeft, hitboxLeft.transform.position, pointerUI.transform.rotation);
        hitboxLeft_Temp.transform.localScale = new Vector3(1.8f, 3f, 1.2f);
        hitboxLeft_Temp.tag = "PlayerMelee";
        hitboxLeft_Temp.GetComponent<MeleeController>().StartTimer();
        
        if(tempDirection == AttackDirection.Right) {
            hitboxLeft_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Right);
        }

        else {
            hitboxLeft_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Left);
        }

        hitboxLeft_Temp.SetActive(true);
    }

    void InitHitBoxLunge() {
        hitboxLunge_Temp = Instantiate(hitboxLunge, hitboxLunge.transform.position, pointerUI.transform.rotation);
        hitboxLunge_Temp.transform.localScale = new Vector3(2.615041f, 5.071505f, 1.2f);
        hitboxLunge_Temp.tag = "PlayerMelee";
    }

    void InitHitBoxDetain()
    {
        hitboxDetain_Temp = Instantiate(hitboxDetain, hitboxDetain.transform.position, pointerUI.transform.rotation);
        hitboxDetain_Temp.transform.localScale = new Vector3(1.8f, 3f, 1.2f);
        hitboxDetain_Temp.tag = "PlayerMelee";
        hitboxDetain_Temp.GetComponent<MeleeController>().StartTimer();

        if (tempDirection == AttackDirection.Right)
        {
            hitboxDetain_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Right);
        }

        else
        {
            hitboxDetain_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Left);
        }

        hitboxDetain_Temp.SetActive(true);
    }

    void LungePlayer() {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 10f;
        if(Gamepad.all.Count == 0) rb.AddForce(tempPos.ToIso() * combat.lungeForce * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
        else {
            rb.AddForce(tempVector.ToIso().normalized * combat.lungeForce * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        
    }

    void LungePlayer(float modifier) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 10f;
        float tempForce = combat.lungeForce + (combat.lungeForce * modifier);
        if(Gamepad.all.Count == 0) rb.AddForce(tempPos.ToIso() * tempForce * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
        else {
            rb.AddForce(tempVector.ToIso() * combat.lungeForce * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
        }
        
    }

    void LungePlayerAlt() {
        if(timerFlickState == TimerState.Start) {
            tempflicktime -= Time.deltaTime;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.drag = 10f;
            if(Gamepad.all.Count == 0) rb.AddForce(tempPosition.ToIso() * combat.quicklungeForce * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
            else {
                rb.AddForce(tempVect.ToIso() * combat.quicklungeForce * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
            }

            if(tempflicktime <= 0) {
                timerFlickState = TimerState.Stop;
            }
        }
    }

    void UpdateTimer() {
        if(timerState == TimerState.Start) {
            attackUISlider.value = temptime;
            PlayerData.isAttacking = true;
            PlayerData.entityState = EntityState.BasicAttack;
        }
        if(timerState == TimerState.Stop) {
            attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f);
            attackUISlider.value = 0;

            //Set States
            timerState = TimerState.None;
        }

        if(timerState == TimerState.None) {
            PlayerData.isAttacking = false;
            PlayerData.entityDirection = deltaDir;
            PlayerData.entityState = deltaState;
        }
    }


    //Pointer
    void UpdatePointer() {
        pointerUI.transform.position = new Vector3(this.transform.position.x, 0.05f, this.transform.position.z);
        if(Gamepad.all.Count == 0) toIsoRotation();
        rot = Quaternion.Euler(rotX, -angle-45, 0.0f);
        pointerUI.transform.rotation = rot;
    }

    void toIsoRotation() {
        tempVector = Camera.main.WorldToScreenPoint(pointerUI.transform.position);
        tempVector = Input.mousePosition - tempVector;
        angle = Mathf.Atan2(tempVector.y, tempVector.x) * Mathf.Rad2Deg;
    }

    void toIsoRotation_Gamepad(Parameters parameters)
    {
        RStickInput = parameters.GetVector3Extra(RSTICK, Vector3.zero);
        if(RStickInput != Vector3.zero) {
            tempVector = new Vector3(RStickInput.x, 0f, RStickInput.z); 
            angle = Mathf.Atan2(tempVector.z, tempVector.x) * Mathf.Rad2Deg; 
        }
        
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
}
