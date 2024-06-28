using Sirenix.OdinInspector;
using UnityEngine;
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
    [ReadOnly] [HideLabel] public Vector3 tempPosition;

    [BoxGroup("ShowDebug/Debug")]
    [ReadOnly] [HideLabel] public float tempflicktime;

    [BoxGroup("ShowDebug/Debug")]
    [ReadOnly] [HideLabel] public Vector3 tempPos;

    [Space] [TitleGroup("Miscallaneous", "[For Debug Purposes]", alignment: TitleAlignments.Split)]
    public bool BasicAttack;

    [ShowIfGroup("BasicAttack")]
    [BoxGroup("BasicAttack/BasicAttack")]
    [ReadOnly] public int counter = 0;

    [BoxGroup("BasicAttack/BasicAttack")]
    [ReadOnly] public bool leftClick;

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
        hitboxLeft.SetActive(false);

    }

    void Start() {
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttackState);
    }

    void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
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
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR_1")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR_2")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR_3")) {
            counter = 0;
            timerState = TimerState.Stop;
        }


        //Left
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL_1")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL_2")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL_3")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
    }

    //Basic Attack
    void BasicAttackState(Parameters parameters) {
        leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);
        
        if(leftClick && IsMouseOverGameWindow) {
            PlayerData.isAttacking = true;
            deltaState = PlayerData.entityState;
            deltaDir = PlayerData.entityDirection;

            timerState = TimerState.Start;
            counter++;

            tempDirection = attackDirection;
            
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
            InitHitBoxLunge();
        }

        counter = Mathf.Clamp(counter, 0, 3);

        SwitchAnimation();
        UpdateLunge();
    }

    void SwitchAnimation() {
        //1st Move
        if(counter == 1) {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR_1");
            else attackAnimator.Play("BasicAtkL_1");
        }

        //2nd Move
        if(counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR_1")) {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR_2");
            else attackAnimator.Play("BasicAtkL_2");
        }

        if(counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL_1")) {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR_2");
            else attackAnimator.Play("BasicAtkL_2");
            
        }

        //3rd Move
        if(counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR_2")) {
            tempflicktime = combat.flicktime;
            timerFlickState = TimerState.Start;

            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR_3");
            else attackAnimator.Play("BasicAtkL_3");
        }

        if(counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL_2")) {
            tempflicktime = combat.flicktime;
            timerFlickState = TimerState.Start;

            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR_3");
            else attackAnimator.Play("BasicAtkL_3");
        }
    }

    void UpdateLunge() {
        //3rd Move
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkR_3")) {
            if(hitboxLunge_Temp != null) {
                hitboxLunge_Temp.GetComponent<MeleeController>().StartTimer();
                hitboxLunge_Temp.SetActive(true);
            }
            
            LungePlayerAlt();
        }

        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL_3")) {
            if(hitboxLunge_Temp != null) {
                hitboxLunge_Temp.GetComponent<MeleeController>().StartTimer();
                hitboxLunge_Temp.SetActive(true);
            }
            LungePlayerAlt();
        }
    }

    Vector3 GetTempPosition() {
        return tempPos;
    }

    void InitHitBoxLeft() {
        hitboxLeft_Temp = Instantiate(hitboxLeft, hitboxLeft.transform.position, pointerUI.transform.rotation);
        hitboxLeft_Temp.transform.localScale = new Vector3(1.8f, 3f, 1.2f);
        hitboxLeft_Temp.tag = "PlayerMelee";
        hitboxLeft_Temp.GetComponent<MeleeController>().StartTimer();
        hitboxLeft_Temp.SetActive(true);
    }

    void InitHitBoxLunge() {
        hitboxLunge_Temp = Instantiate(hitboxLunge, hitboxLunge.transform.position, pointerUI.transform.rotation);
        hitboxLunge_Temp.transform.localScale = new Vector3(2.615041f, 5.071505f, 1.2f);
        hitboxLunge_Temp.tag = "PlayerMelee";
    }

    void LungePlayer() {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 10f;
        rb.AddForce(tempPos.ToIso() * combat.lungeForce, ForceMode.Impulse);
    }

    void LungePlayer(float modifier) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 10f;
        float tempForce = combat.lungeForce + (combat.lungeForce * modifier);
        rb.AddForce(tempPos.ToIso() * tempForce, ForceMode.Impulse);
    }

    void LungePlayerAlt() {
        if(timerFlickState == TimerState.Start) {
            tempflicktime -= Time.deltaTime;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.mass = 0.1f;
            rb.drag = 10f;
            rb.AddForce(tempPosition.ToIso() * combat.quicklungeForce, ForceMode.VelocityChange);
            if(tempflicktime <= 0) {
                rb.mass = 1f;
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
}
