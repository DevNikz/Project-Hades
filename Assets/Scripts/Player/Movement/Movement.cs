using Sirenix.OdinInspector;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    [PropertySpace] [Title("Movement")]
    [AssetSelector(Paths = "Assets/Data/Player/Movement")]
    public PlayerMovement movement;

    [Space] [Title("References")]
    public bool ShowReference;

    [ShowIfGroup("ShowReference")]
    [BoxGroup("ShowReference/References")]
    [Tooltip("Set Rigidbody reference of the GameObject")]
    [SerializeField] private Rigidbody rigidBody;

    [BoxGroup("ShowReference/References")]
    [Tooltip("Set Transform property reference of the GameObject")]
    [SerializeField] private Transform model;

    //Effects
    [Space] [Title("Experimental Effects")]
    public bool ShowEffects;
    
    [ShowIfGroup("ShowEffects")]
    [BoxGroup("ShowEffects/Effects")]
    [SerializeField] public ParticleSystem dust;

    [BoxGroup("ShowEffects/Effects")]
    [SerializeField] private ParticleSystem dashParticle;

    //Input References
    [Space] [Title("Input")]
    public bool ShowInput;

    [ShowIfGroup("ShowInput")]
    [BoxGroup("ShowInput/Input")]
    [ReadOnly] [SerializeReference] private Vector3 moveInput;
    [BoxGroup("ShowInput/Input")]
    [ReadOnly] [SerializeReference] private float moveInput_normalized;

    [BoxGroup("ShowInput/Input")]
    [ReadOnly] [SerializeReference] private bool dashInput;

    [Space] [Title("Speed")]
    public bool ShowSpeed;
    [ShowIfGroup("ShowSpeed")]
    [BoxGroup("ShowSpeed/Speed")]
    [ReadOnly] public float currentSpeed;

    [BoxGroup("ShowSpeed/Speed")]
    [ReadOnly] public Vector3 currentSpeedDebug;

    [Space] [Title("Dash")]
    public bool ShowDash;
    
    [ShowIfGroup("ShowDash")]
    [BoxGroup("ShowDash/Dash")]
    [ReadOnly] public bool dashing;

    [BoxGroup("ShowDash/Dash")]
    [ReadOnly] public float dashCDTimer;

    [BoxGroup("ShowDash/Dash")]
    [ReadOnly] public Vector3 delayedForce;

    [BoxGroup("ShowDash/Dash")]
    [ReadOnly] public Vector3 isoInput;

    //States
    [Space] [Title("States")]
    public bool ShowStates;
    [ShowIfGroup("ShowStates")]
    [BoxGroup("ShowStates/States")]
    [SerializeReference] public EntityState state;
    [BoxGroup("ShowStates/States")]
    [SerializeReference] public EntityDirection direction;

    //Broadcaster
    public const string KEY_MOVE = "KEY_MOVE";
    
    public const string KEY_DASH = "KEY_DASH";

    public const string KEY_MOVE_HELD = "KEY_MOVE_HELD";
    public const string RIGHT_CLICK = "RIGHT_CLICK";

    void Awake() {
        movement = Resources.Load<PlayerMovement>("Player/Movement/PlayerMovement");
        rigidBody = this.GetComponent<Rigidbody>();
        model = this.GetComponent<Transform>();
        dust = transform.Find("GroundDust").gameObject.GetComponent<ParticleSystem>();
        dust.Play();

        this.GetComponent<CapsuleCollider>().material = Resources.Load<PhysicMaterial>("Player/Player");

        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.KEY_INPUTS, this.moveEvent);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.KEY_INPUTS, this.stateHandlerEvent);
    }

    void OnEnable() {
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.KEY_INPUTS, this.moveEvent);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.KEY_INPUTS, this.stateHandlerEvent);
    }

    void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.KEY_INPUTS);
    }

    private void Update() {
        //Checks
        CheckDrag();
        CheckMove();
        CheckDash();

        //Init Dash Funcs
        Cooldown();
    }

    private void moveEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);
        
        if(dashInput || PlayerData.isAttacking ) return;
        else{
            moveInput_normalized = moveInput.normalized.magnitude;
            currentSpeedDebug = transform.localPosition + moveInput.ToIso() * moveInput_normalized * currentSpeed * Time.fixedDeltaTime;
            rigidBody.MovePosition(transform.localPosition + moveInput.ToIso() * moveInput_normalized * currentSpeed * Time.fixedDeltaTime);
        }
    }

    private void CheckMove() {
        ParticleSystem.EmissionModule temp = dust.emission;
        if(state == EntityState.Strafing) temp.enabled = true;
        else temp.enabled = false;
    }

    private void CheckDash() {
        if(state == EntityState.Dashing) dashParticle.Play();
    }

    private void stateHandlerEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);
        dashInput = parameters.GetBoolExtra(KEY_DASH, false);

        if(dashInput == true) {
            currentSpeed = movement.dashSpeed;
            Dash();
        }

        else if(moveInput.x != 0 || moveInput.z != 0) {
            //Set To Strafing
            state = EntityState.Strafing;
            PlayerData.entityState = EntityState.Strafing;

            //Set To Strafing Speed
            currentSpeed = movement.strafeSpeed;

            //Debug Direction
            direction = IsoCompass(moveInput.x, moveInput.z);
        }

        else if(moveInput.x == 0 && moveInput.z == 0) {
            state = EntityState.Idle;
            PlayerData.entityState = EntityState.Idle;
        }

        else if(PlayerData.entityState == EntityState.BasicAttack){
            state = EntityState.BasicAttack;
            PlayerData.entityState = EntityState.BasicAttack;
        }
    }

    private void CheckDrag() {
        if(state == EntityState.Strafing) {
            rigidBody.drag = movement.groundDrag;
        }
        else rigidBody.drag = 10f;
    }

    private EntityDirection IsoCompass(float x, float z) {
        //North
        if(x == 0 && (z <= 1 && z > 0)) {
            PlayerData.animDirection = AnimDirection.Up;
            PlayerData.entityDirection = EntityDirection.North;
            return EntityDirection.North;
        }

        //North East
        else if((x <= 1 && x > 0) && (z <= 1 && z > 0)) {
            PlayerData.animDirection = AnimDirection.Up;
            PlayerData.entityDirection = EntityDirection.NorthEast;
            return EntityDirection.NorthEast;
        }

        //East
        else if((x <= 1 && x > 0) && z == 0) {
            PlayerData.animDirection = AnimDirection.Left;
            PlayerData.entityDirection = EntityDirection.East;
            return EntityDirection.East;
        }

        //South East
        else if((x <= 1 && x > 0) && (z >= -1 && z < 0)) {
            PlayerData.animDirection = AnimDirection.Down;
            PlayerData.entityDirection = EntityDirection.SouthEast;
            return EntityDirection.SouthEast;
        }

        //South
        else if(x == 0 && (z >= -1 && z < 0)) {
            PlayerData.animDirection = AnimDirection.Down;
            PlayerData.entityDirection = EntityDirection.South;
            return EntityDirection.South;
        }

        //South West
        else if((x >= -1 && x < 0) && (z >= -1 && z < 0)) {
            PlayerData.animDirection = AnimDirection.Down;
            PlayerData.entityDirection = EntityDirection.SouthWest;
            return EntityDirection.SouthWest;
        }

        //West
        else if((x >= -1 && x < 0) && z == 0) {
            PlayerData.animDirection = AnimDirection.Right;
            PlayerData.entityDirection = EntityDirection.West;
            return EntityDirection.West;
        }

        //North West
        else if((x >= -1 && x < 0) && (z <= 1 && z > 0)) {
            PlayerData.animDirection = AnimDirection.Up;
            PlayerData.entityDirection = EntityDirection.NorthWest;
            return EntityDirection.NorthWest;
        }

        else {
            PlayerData.entityDirection = EntityDirection.None;
            return EntityDirection.None;
        }
    }

    private void Dash() {
        if(dashCDTimer > 0 ) return;
        else dashCDTimer = movement.dashCD;

        //Set Dash To True
        dashing = true;

        //Convert World View Coords To Iso Coords
        isoInput = this.ConvertToIso(moveInput.x, moveInput.z);

        //Apply Dash Based On KeyInput
        delayedForce = isoInput * movement.dashForce; 

        //Duration
        Invoke(nameof(DelayedDashForce), movement.dashDuration);

        //Cooldown
        Invoke(nameof(ResetDash), movement.dashDuration);
    }

    private Vector3 ConvertToIso(float x, float z) {

        //North
        if(x == 0 && (z <= 1 && z > 0)) return new Vector3(1f, 0f, 1f);

        //North East
        else if((x <= 1 && x > 0) && (z <= 1 && z > 0)) return new Vector3(1f, 0f, 0f);

        //East
        else if((x <= 1 && x > 0) && z == 0) return new Vector3(1f, 0f, -1f);

        //South East
        else if((x <= 1 && x > 0) && (z >= -1 && z < 0)) return new Vector3(0f, 0f, -1f);

        //South
        else if(x == 0 && (z >= -1 && z < 0)) return new Vector3(-1f, 0f, -1f);

        //South West
        else if((x >= -1 && x < 0) && (z >= -1 && z < 0)) return new Vector3(-1f, 0f, 0f);

        //West
        else if((x >= -1 && x < 0) && z == 0) return new Vector3(-1f, 0f, 1f);

        //North West
        else if((x >= -1 && x < 0) && (z <= 1 && z > 0)) return new Vector3(0f, 0f, 1f);

        else {
            Vector3 zero = Vector3.zero;
            return zero;
        }
    }

    private void DelayedDashForce() {
        state = EntityState.Dashing;
        PlayerData.entityState = EntityState.Dashing;
        rigidBody.AddForce(delayedForce * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void ResetDash() {
        dashing = false;
    }

    private void Cooldown() {
        if(dashCDTimer > 0) dashCDTimer -= Time.fixedDeltaTime;
    }
}

