using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Combat : MonoBehaviour
{
    //Reference
    private GameObject pointerUI;
    private GameObject movementSprite;
    private GameObject attackSprite;
    private Animator attackAnimator;
    public AttackDirection attackDirection;
    public AttackDirection tempDirection;

    //Pointer
    private Vector3 tempVector;
    public float angle;
    private Quaternion rot;
    private float rotX;
    //public float tempAngle;

    //Basic Attack (Left Click)
    [Header("Basic Attack")]
    [SerializeField] [Range(10f,20f)] private float lungeForce; 
    [SerializeField] private int counter = 0;
    private Vector3 tempPos;
    private bool leftClick;
    public const string LEFT_CLICK = "LEFT_CLICK";
    
    //Alternate Attack(Right Click)
    [Header("Alternate Attack")]
    private bool rightClick;
    public const string RIGHT_CLICK = "RIGHT_CLICK";

    //Timer
    [Header("Timer")]
    [SerializeField] private TimerState timerState;
    [SerializeField] [Range(0.1f,5f)] private float timer;
    [SerializeField] private float tickingTimer;

    void Awake() {
        //Reference
        pointerUI = transform.Find("Pointer").gameObject;
        movementSprite = transform.Find("Sprites").transform.Find("Movement").gameObject;
        attackSprite = transform.Find("Sprites").transform.Find("Attack").gameObject;
        attackAnimator = attackSprite.GetComponent<Animator>();
        rotX = pointerUI.transform.rotation.eulerAngles.x;
        timerState = TimerState.None;

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

        if(PlayerData.entityState == EntityState.BasicAttack) UpdateAnimation();
    }

    void UpdateAttackDirection() {
        if(angle >= 0 && angle <= 90) attackDirection = AttackDirection.Right;
        else if(angle <= 0 && angle >= -90) attackDirection = AttackDirection.Right;
        else attackDirection = AttackDirection.Left;
    }

    void UpdateAnimation() {
        //Right
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk1_R")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk2_R")) {
            counter = 0;
            timerState = TimerState.Stop;
        }

        //Left
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk1_L")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
        if(attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk2_L")) {
            counter = 0;
            timerState = TimerState.Stop;
        }
    }

    //Basic Attack
    void BasicAttack(Parameters parameters) {
        leftClick = parameters.GetBoolExtra(LEFT_CLICK, false);
        
        if(leftClick) {
            movementSprite.SetActive(false);
            attackSprite.SetActive(true);

            tickingTimer = timer;
            timerState = TimerState.Start;
            counter++;

            tempDirection = attackDirection;
            
            LungePlayer();
        }

        if(counter == 1) {
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtk1_R");
            else attackAnimator.Play("BasicAtk1_L");
        }

        counter = Mathf.Clamp(counter, 0, 2);

        if(counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk1_R")) {
            tickingTimer = timer - 0.2f;
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtk2_R");
            else attackAnimator.Play("BasicAtk2_L");
        }

        else if(counter >= 2 && attackAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && attackAnimator.GetCurrentAnimatorStateInfo(0).IsName("BasicAtk1_L")) {
            tickingTimer = timer - 0.2f;
            if(tempDirection == AttackDirection.Right) attackAnimator.Play("BasicAtk2_R");
            else attackAnimator.Play("BasicAtk2_L");
        }
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
            PlayerData.entityState = EntityState.BasicAttack;
            tickingTimer -= Time.deltaTime;
            if(tickingTimer <= 0f) {
                Debug.Log("Stopping Time.");
                counter = 0;
                timerState = TimerState.Stop;
            }
        }
        if(timerState == TimerState.Stop) {
            timerState = TimerState.None;
            //Set Actives
            movementSprite.SetActive(true);
            attackSprite.SetActive(false);

            //Set States
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.drag = 10f;
            PlayerData.entityState = EntityState.Idle;
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
