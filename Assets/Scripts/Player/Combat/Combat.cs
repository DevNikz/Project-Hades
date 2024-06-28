using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    //Reference
    private GameObject pointerUI;
    private GameObject attackUI;
    private Slider attackUISlider;
    private RectTransform attackUIEnd;
    private Animator attackAnimator;
    private AttackDirection attackDirection;
    private AttackDirection tempDirection;
    private EntityState deltaState;
    private EntityDirection deltaDir;

    //Pointer
    private Vector3 tempVector;
    public float angle;
    private Quaternion rot;
    private float rotX;

    //Basic Attack (Left Click)
    [Header("Basic Attack")]
    [SerializeField] [Range(10f,50f)] private float lungeForce; 
    [SerializeField] [Range(0.1f,20f)] private float quicklungeForce;
    public Vector3 tempPosition;
    [Range(0.1f,2f)] public float flicktime = 1f;
    private float tempflicktime;
    [SerializeField] private int counter = 0;
    private Vector3 tempPos;
    private bool leftClick;
    private GameObject hitboxLeft;
    private GameObject hitboxLunge;
    private GameObject hitboxLeft_Temp;
    private GameObject hitboxLunge_Temp;
    public const string LEFT_CLICK = "LEFT_CLICK";
    
    //Alternate Attack(Right Click)
    [Header("Alternate Attack")]
    private bool rightClick;
    public const string RIGHT_CLICK = "RIGHT_CLICK";

    //Timer
    [Header("Timer")]
    [SerializeField] private TimerState timerState;
    [SerializeField] private TimerState timerFlickState;
    public float temptime;

    void Awake() {
        //Reference
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
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.BasicAttack);
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
    void BasicAttack(Parameters parameters) {
        leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);
        
        if(leftClick) {
            PlayerData.isAttacking = true;
            deltaState = PlayerData.entityState;
            deltaDir = PlayerData.entityDirection;

            timerState = TimerState.Start;
            counter++;

            tempDirection = attackDirection;
            
        }

        if(leftClick && counter > 0 && counter <= 2) {
            InitHitBoxLeft();
            LungePlayer();
        }
        
        if(leftClick && counter == 3) {
            tempPosition = GetTempPosition();
            InitHitBoxLunge();
        }

        if(counter == 1) {
            //attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR_1");
            else attackAnimator.Play("BasicAtkL_1");
        }

        counter = Mathf.Clamp(counter, 0, 3);

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
            tempflicktime = flicktime;
            timerFlickState = TimerState.Start;

            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR_3");
            else attackAnimator.Play("BasicAtkL_3");
        }

        if(counter >= 3 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtkL_2")) {
            tempflicktime = flicktime;
            timerFlickState = TimerState.Start;

            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtkR_3");
            else attackAnimator.Play("BasicAtkL_3");
        }

        //3rd Move Lunge
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
        //Lunge
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 10f;
        rb.AddForce(tempPos.ToIso() * lungeForce, ForceMode.Impulse);
    }

    void LungePlayerAlt() {
        if(timerFlickState == TimerState.Start) {
            tempflicktime -= Time.deltaTime;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.mass = 0.1f;
            rb.drag = 10f;
            rb.AddForce(tempPosition.ToIso() * quicklungeForce, ForceMode.VelocityChange);
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
}
