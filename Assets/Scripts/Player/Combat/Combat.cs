using Sirenix.OdinInspector;
using TMPro;
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

    [BoxGroup("ShowTimer/TimerSettings")]
    [ReadOnly] public float detainTimer;

    [BoxGroup("ShowTimer/TimerSettings")]
    [SerializeField] public float detainCooldown;

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

    [BoxGroup("Reference/References")]
    [SerializeField] public TextMeshProUGUI fireChargeText;

    [Title("Elemental Charges")]
    [SerializeField][Range(0.1f, 100f)] public float maxFireCharge;
    [SerializeField] private float currentFireCharge;

    //Broadcaster
    public const string LEFT_CLICK = "LEFT_CLICK";
    public const string RIGHT_CLICK = "RIGHT_CLICK";
    public const string RSTICK = "RSTICK";
    public const string DETAIN = "DETAIN";
    public const string HIDDEN = "HIDDEN";
    public const string ENEMY_KILLED = "ENEMY_KILLED";

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

        fireChargeText.text = "Current Fire Charge: " + currentFireCharge.ToString();
        detainCooldown = 5.0f;
    }

    void OnEnable() {
        hitboxLeft.SetActive(false);
        hitboxLunge.SetActive(false);
        hitboxDetain.SetActive(false);
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        timerState = TimerState.None;
        temptime = 0;
        counter = 0;
        detainTimer = 0;
        Debug.Log("Combat Enabled!");
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState);
        EventBroadcaster.Instance.AddObserver(EventNames.GamepadInput.RIGHT_STICK_INPUT, this.toIsoRotation_Gamepad);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.DETAIN_PRESS, this.DetainAttackState);
        EventBroadcaster.Instance.AddObserver(EventNames.Combat.PLAYER_SEEN, this.SetPlayerSeen);
        EventBroadcaster.Instance.AddObserver(EventNames.Combat.ENEMY_KILLED, this.UpdateFireCharge);
    }

    void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.GamepadInput.RIGHT_STICK_INPUT);
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.DETAIN_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Combat.PLAYER_SEEN);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Combat.ENEMY_KILLED);
    }

    void Update() {
        UpdatePointer();
        UpdateTimer();
        UpdateAttackDirection();
        SwitchWeapon();
        
        //Temp
        tempPos = new Vector3(tempVector.x, this.transform.position.y, tempVector.y).normalized;

        if (PlayerData.isAttacking && MenuScript.LastSelection == 3)
        {
            UpdateAnimation();
        }
        else if (PlayerData.isAttacking && MenuScript.LastSelection == 2)
        {
            UpdateFireAnimation();

        }
        else if (PlayerData.isAttacking)
        {
            UpdateAnimation();
        }
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

    void UpdateFireAnimation()
    {
        temptime = attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;    
        //Right
        if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR1"))
        {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR2"))
        {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR3"))
        {
            counter = 0;
            timerState = TimerState.Stop;
        }


        //Left
        if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL1"))
        {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL2"))
        {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if (attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL3"))
        {
            counter = 0;
            timerState = TimerState.Stop;
        }
    }

    void SwitchWeapon()
    {
        // Example usage of the last selection
        int lastWeapon = MenuScript.LastSelection;


        // Do something based on the last selection
        switch (lastWeapon)
        {
            case 2:
                EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
                EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.FireAttack);
                break;
            case 3:
                EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
                EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState);
                break;
            default:
                break;
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

    void FireAttack(Parameters parameters)
    {
        leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);

    

        if (Gamepad.all.Count == 0)
        {
            if (leftClick && IsMouseOverGameWindow)
            {
                PlayerData.isAttacking = true;

                deltaState = PlayerData.entityState;
                deltaDir = PlayerData.entityDirection;

                timerState = TimerState.Start;
                counter++;

                tempDirection = attackDirection;
            }
        }
        else
        {
            if (leftClick)
            {
                PlayerData.isAttacking = true;
                deltaState = PlayerData.entityState;
                deltaDir = PlayerData.entityDirection;

                timerState = TimerState.Start;
                counter++;

                tempDirection = attackDirection;



            }
        }

        if (leftClick && counter == 1)
        {
            InitHitBoxLeft();
            LungePlayer();
        }

        if (leftClick && counter == 2)
        {
            InitHitBoxLeft();
            LungePlayer(combat.lungeForceMod);
        }

        if (leftClick && counter == 3)
        {
            tempPosition = GetTempPosition();
            tempVect = GetTempVector();
            InitHitBoxLunge();
        }

        counter = Mathf.Clamp(counter, 0, 3);

        SwitchFireAnimation();
        UpdateLunge();
    }

    //DetainAttack (copy of basic attack for now)
    void DetainAttackState(Parameters parameters)
    {
        detainPress = parameters.GetBoolExtra(DETAIN, false);
        

        if(detainPress && !this.playerSeen)
        {
            if(this.detainTimer >= this.detainCooldown)
            {
                this.detainTimer = 0;

                if (Gamepad.all.Count == 0)
                {
                    PlayerData.isAttacking = true;
                    deltaState = PlayerData.entityState;
                    deltaDir = PlayerData.entityDirection;
                    
                    timerState = TimerState.Start;
                    counter = 1;

                    tempDirection = attackDirection;
                }

                InitHitBoxDetain();
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

        counter = Mathf.Clamp(counter, 0, 3);

        SwitchAnimation();
        UpdateLunge();
    }

    void SwitchAnimation() {
        //1st Move
        //The other conditions are for the unanimated aspects. They're just here to prevent some jank while doing the demo.
        if(counter == 1 && (MenuScript.LastSelection == 3 || MenuScript.LastSelection == 0 || MenuScript.LastSelection == 1 || MenuScript.LastSelection == 4) ) {
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

    void SwitchFireAnimation()
    {
        //1st Move
        if (counter == 1 && MenuScript.LastSelection == 2)
        {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f);
            if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR1");
            else attackAnimator.Play("FireAtkL1");

        }

        //2nd Move
        if (counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR1"))
        {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
            if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR2");
            else attackAnimator.Play("FireAtkL2");
        }

        if (counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL1"))
        {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
            if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR2");
            else attackAnimator.Play("FireAtkL2");

        }

        //3rd Move
        if (counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR2"))
        {
            tempflicktime = combat.flicktime;
            timerFlickState = TimerState.Start;

            if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR3");
            else attackAnimator.Play("FireAtkL3");
        }

        if (counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL2"))
        {
            tempflicktime = combat.flicktime;
            timerFlickState = TimerState.Start;

            if (tempDirection == AttackDirection.Right) attackAnimator.Play("FireAtkR3");
            else attackAnimator.Play("FireAtkL3");
        }
    }

    void UpdateLunge() {
        //3rd Move
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && (attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR3_New")||attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkR3"))) {
            if(hitboxLunge_Temp != null) {
                hitboxLunge_Temp.GetComponent<MeleeController>().StartTimer();
                hitboxLunge_Temp.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Right);
                hitboxLunge_Temp.SetActive(true);
            }
            
            LungePlayerAlt();
        }

        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && (attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL3_New")| attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireAtkL3"))) {
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

        //Hijacked for Detain Cooldown :)
        if(this.detainTimer < this.detainCooldown)
            this.detainTimer += Time.deltaTime;
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

    void UpdateFireCharge(Parameters param)
    {
        bool enemyKilled = param.GetBoolExtra(ENEMY_KILLED, false);

        if (enemyKilled && (currentFireCharge < maxFireCharge) && detainPress)
        {
            Debug.Log("Fire charge update!");
            currentFireCharge += 20;
        }

        fireChargeText.text = "Current Fire Charge: " + currentFireCharge.ToString();
    }
}
