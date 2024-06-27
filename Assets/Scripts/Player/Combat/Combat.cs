using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    //Reference
    private GameObject pointerUI;
    private GameObject attackUI;
    private Slider attackUISlider;
    private RectTransform attackUIEnd;
    private GameObject movementSprite;
    private GameObject attackSprite;
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
    [SerializeField] [Range(10f,20f)] private float lungeForce; 
    [SerializeField] private int counter = 0;
    private Vector3 tempPos;
    private bool leftClick;
    private GameObject hitboxLeft;
    private GameObject hitboxLeft_Temp;
    public const string LEFT_CLICK = "LEFT_CLICK";
    
    //Alternate Attack(Right Click)
    [Header("Alternate Attack")]
    private bool rightClick;
    public const string RIGHT_CLICK = "RIGHT_CLICK";

    //Timer
    [Header("Timer")]
    [SerializeField] private TimerState timerState;
    public float temptime;

    void Awake() {
        //Reference
        pointerUI = transform.Find("Pointer").gameObject;
        attackUI = transform.Find("AttackUI").gameObject;
        attackUISlider = attackUI.transform.Find("Border").transform.Find("StartBase").transform.Find("Slider").GetComponent<Slider>();
        attackUIEnd = attackUI.transform.Find("Border").transform.Find("EndBase").transform.Find("End").GetComponent<RectTransform>();
        movementSprite = transform.Find("Sprites").transform.Find("Movement").gameObject;
        attackSprite = transform.Find("Sprites").transform.Find("Attack").gameObject;
        attackAnimator = attackSprite.GetComponent<Animator>();
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        timerState = TimerState.None;

        hitboxLeft = pointerUI.transform.Find("Melee").gameObject;
        hitboxLeft.SetActive(false);

        //Actives
        movementSprite.SetActive(true);
        attackSprite.SetActive(false);
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
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk1_R")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk2_R")) {
            counter = 0;
            timerState = TimerState.Stop;
        }

        //Left
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk1_L")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk2_L")) {
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

            movementSprite.SetActive(false);
            attackSprite.SetActive(true);

            tempDirection = attackDirection;
            
        }

        if(leftClick && counter > 0) {
            InitHitBoxLeft();
            LungePlayer();
        }

        if(counter == 1) {
            attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtk1_R");
            else attackAnimator.Play("BasicAtk1_L");
        }

        counter = Mathf.Clamp(counter, 0, 2);

        if(counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk1_R")) {
            attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtk2_R");
            else attackAnimator.Play("BasicAtk2_L");
        }

        else if(counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.85f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk1_L")) {
            attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f+15f);
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtk2_R");
            else attackAnimator.Play("BasicAtk2_L");
        }
    }

    void InitHitBoxLeft() {
        hitboxLeft_Temp = Instantiate(hitboxLeft, hitboxLeft.transform.position, pointerUI.transform.rotation);
        hitboxLeft_Temp.transform.localScale = new Vector3(1.8f, 3f, 1.2f);
        hitboxLeft_Temp.tag = "PlayerMelee";
        hitboxLeft_Temp.SetActive(true);
    }

    void LungePlayer() {
        //Debug
        Debug.Log("Lunging!");

        //Lunge
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0f;
        rb.AddForce(tempPos.ToIso() * lungeForce, ForceMode.Impulse);
    }

    void UpdateTimer() {
        if(timerState == TimerState.Start) {
            // meterValue += 23.5f;
            // attackUIsize.sizeDelta = new Vector2(attackUIsize.sizeDelta.x, meterValue);
            attackUISlider.value = temptime;
            PlayerData.isAttacking = true;
            PlayerData.entityState = EntityState.BasicAttack;
        }
        if(timerState == TimerState.Stop) {
            // meterValue = 0;
            // attackUIsize.sizeDelta = new Vector2(attackUIsize.sizeDelta.x, meterValue);
            attackUIEnd.sizeDelta = new Vector2(attackUIEnd.sizeDelta.x, 23.5f);
            attackUISlider.value = 0;

            //Set Actives
            movementSprite.SetActive(true);
            attackSprite.SetActive(false);

            //Set States
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.drag = 10f;
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
