using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    //Properties
    [Header("Properties")]
    [Tooltip("Set Rigidbody reference of the GameObject")]
    [SerializeField] private Rigidbody rigidBody;
    [Tooltip("Set Transform property reference of the GameObject")]
    [SerializeField] private Transform model;

    //Strafe
    [Header("Speed")]
    [SerializeField] private Strafe strafe;
    
    //States
    [Header("State")]
    [SerializeField] private EntityState state = EntityState.Idle;
    [ReadOnly] public EntityDirection direction = EntityDirection.None;

    //Dashing
    [Header("Dash")]
    [SerializeField] private Dash dash;

    //Keybinds
    [Header("Keybinds")]
    private bool rightClick;
    [ReadOnly] protected string debugString = "W.I.P";

    public const string KEY_MOVE = "KEY_MOVE";
    
    public const string KEY_DASH = "KEY_DASH";

    public const string KEY_MOVE_HELD = "KEY_MOVE_HELD";
    public const string RIGHT_CLICK = "RIGHT_CLICK";

    //Effects
    [Header("Experimental Effects")]
    
    [SerializeField] public ParticleSystem dust;

    [SerializeField] private ParticleSystem dashParticle;

    [SerializeField] public bool moveHeld;

    //Input References
    private Vector3 moveInput;
    private bool dashInput;

    void Reset() {
        rigidBody = this.GetComponent<Rigidbody>();
        model = this.GetComponent<Transform>();

        state = EntityState.Idle;
        direction = EntityDirection.None;

        strafe.currentSpeed = 5;
        strafe.strafeSpeed = 5;
        strafe.turnSpeed = 720;
        strafe.groundDrag = 10;

        dash.dashSpeed = 10;
        dash.dashing = false;
        dash.dashForce = 25;
        dash.dashDuration = 0.025f;
        dash.dashCD = 1.5f;
    }

    void Start() {
        rigidBody = this.GetComponent<Rigidbody>();
        model = this.GetComponent<Transform>();
        dust = transform.Find("GroundDust").gameObject.GetComponent<ParticleSystem>();
        dust.Play();

        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.KEY_INPUTS, this.moveEvent);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.KEY_INPUTS, this.lookEvent);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.KEY_INPUTS, this.stateHandlerEvent);
    }

    void OnDestroy() {
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
        
        if(dashInput || state == EntityState.BasicAttack || PlayerData.entityState == EntityState.BasicAttack) return;
        else {
            rigidBody.MovePosition(transform.position + moveInput.ToIso() * moveInput.normalized.magnitude * strafe.currentSpeed * Time.deltaTime);
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

    private void lookEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);

        if(moveInput == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(moveInput.ToIso(), Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rot, strafe.turnSpeed * Time.deltaTime);
    }

    private void stateHandlerEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);
        dashInput = parameters.GetBoolExtra(KEY_DASH, false);

        if(dashInput == true) {
            strafe.currentSpeed = dash.dashSpeed;
            Dash();
        }

        if(moveInput.x != 0 || moveInput.z != 0) {
            //Set To Strafing
            state = EntityState.Strafing;
            PlayerData.entityState = EntityState.Strafing;

            //Set To Strafing Speed
            strafe.currentSpeed = strafe.strafeSpeed;

            //Debug Direction
            direction = IsoCompass(moveInput.x, moveInput.z);
        }

        if(moveInput.x == 0 && moveInput.z == 0) {
            state = EntityState.Idle;
            PlayerData.entityState = EntityState.Idle;
        }

        if(PlayerData.entityState == EntityState.BasicAttack){
            state = EntityState.BasicAttack;
            PlayerData.entityState = EntityState.BasicAttack;
        }
    }

    private void CheckDrag() {
        if(state == EntityState.Strafing) {
            rigidBody.drag = strafe.groundDrag;
        }
        else rigidBody.drag = 5f;
    }

    private EntityDirection IsoCompass(float x, float z) {
        //North
        if(x == 0 && z == 1) {
            PlayerData.animDirection = AnimDirection.Up;
            PlayerData.entityDirection = EntityDirection.North;
            return EntityDirection.North;
        }

        //North East
        else if(x == 1 && z == 1) {
            PlayerData.animDirection = AnimDirection.Up;
            PlayerData.entityDirection = EntityDirection.NorthEast;
            return EntityDirection.NorthEast;
        }

        //East
        else if(x == 1 && z == 0) {
            PlayerData.animDirection = AnimDirection.Left;
            PlayerData.entityDirection = EntityDirection.East;
            return EntityDirection.East;
        }

        //South East
        else if(x == 1 && z == -1) {
            PlayerData.animDirection = AnimDirection.Down;
            PlayerData.entityDirection = EntityDirection.SouthEast;
            return EntityDirection.SouthEast;
        }

        //South
        else if(x == 0 && z == -1) {
            PlayerData.animDirection = AnimDirection.Down;
            PlayerData.entityDirection = EntityDirection.South;
            return EntityDirection.South;
        }

        //South West
        else if(x == -1 && z == -1) {
            PlayerData.animDirection = AnimDirection.Down;
            PlayerData.entityDirection = EntityDirection.SouthWest;
            return EntityDirection.SouthWest;
        }

        //West
        else if(x == -1 && z == 0) {
            PlayerData.animDirection = AnimDirection.Right;
            PlayerData.entityDirection = EntityDirection.West;
            return EntityDirection.West;
        }

        //North West
        else if(x == -1 && z == 1) {
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
        if(dash.dashCDTimer > 0 ) return;
        else dash.dashCDTimer = dash.dashCD;

        //Set Dash To True
        dash.dashing = true;

        //Convert World View Coords To Iso Coords
        dash.isoInput = this.ConvertToIso(moveInput.x, moveInput.z);

        //Apply Dash Based On KeyInput
        Vector3 forceToApply = dash.isoInput * dash.dashForce; 

        //Apply Force
        dash.delayedForce = forceToApply;

        //Duration
        Invoke(nameof(DelayedDashForce), 0.025f);

        //Cooldown
        Invoke(nameof(ResetDash), dash.dashDuration);
    }

    private Vector3 ConvertToIso(float x, float z) {

        //North
        if(x == 0 && z == 1) return new Vector3(1f, 0f, 1f);

        //North East
        else if(x == 1 && z == 1) return new Vector3(1f, 0f, 0f);

        //East
        else if(x == 1 && z == 0) return new Vector3(1f, 0f, -1f);

        //South East
        else if(x == 1 && z == -1) return new Vector3(0f, 0f, -1f);

        //South
        else if(x == 0 && z == -1) return new Vector3(-1f, 0f, -1f);

        //South West
        else if(x == -1 && z == -1) return new Vector3(-1f, 0f, 0f);

        //West
        else if(x == -1 && z == 0) return new Vector3(-1f, 0f, 1f);

        //North West
        else if(x == -1 && z == 1) return new Vector3(0f, 0f, 1f);

        else {
            Vector3 zero = Vector3.zero;
            return zero;
        }
    }

    private void DelayedDashForce() {
        state = EntityState.Dashing;
        PlayerData.entityState = EntityState.Dashing;
        rigidBody.AddForce(dash.delayedForce, ForceMode.Impulse);
    }

    private void ResetDash() {
        dash.dashing = false;
    }

    private void Cooldown() {
        if(dash.dashCDTimer > 0) dash.dashCDTimer -= Time.deltaTime;
    }
}

