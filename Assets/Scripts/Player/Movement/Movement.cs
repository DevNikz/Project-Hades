using Unity.Collections;
using UnityEditor.Experimental.GraphView;
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
    [ReadOnly] protected string debugString = "W.I.P";

    public const string KEY_MOVE = "KEY_MOVE";
    
    public const string KEY_DASH = "KEY_DASH";

    public const string KEY_MOVE_HELD = "KEY_MOVE_HELD";

    //Effects
    [Header("Experimental Effects")]
    
    [SerializeField] public ParticleSystem dust;

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

        //Init Dash Funcs
        Cooldown();

        //if(Input.GetKeyDown(KeyCode.R)) ClearDust(); 
    }

    private void moveEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);

        rigidBody.MovePosition(transform.position + moveInput.ToIso() * moveInput.normalized.magnitude * strafe.currentSpeed * Time.deltaTime);
    }

    private void CheckMove() {
        ParticleSystem.EmissionModule temp = dust.emission;
        
        if(state == EntityState.Strafing) temp.enabled = true;
        else temp.enabled = false;
        // if(movement == Movement.Strafing) CreateDust(); 
        // else ClearDust(); 
    }
    
    private void CreateDust() {
        moveHeld = true;
        // dust.Play();
    }

    private void ClearDust() {
        moveHeld = false;
        dust.Clear();
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
            state = EntityState.Dashing;
            strafe.currentSpeed = dash.dashSpeed;
            Dash();
        }

        else if(moveInput.x != 0 || moveInput.z != 0) {
            //Set To Strafing
            state = EntityState.Strafing;

            //Set To Strafing Speed
            strafe.currentSpeed = strafe.strafeSpeed;

            //Debug Direction
            direction = IsoCompass(moveInput.x, moveInput.z);
        }

        else state = EntityState.Idle;
    }

    private void CheckDrag() {
        if(state == EntityState.Strafing) {
            rigidBody.drag = strafe.groundDrag;
        }
        else rigidBody.drag = 5f;
    }

    private EntityDirection IsoCompass(float x, float z) {
        //North
        if(x == 0 && z == 1) return EntityDirection.North;

        //North East
        else if(x == 1 && z == 1) return EntityDirection.NorthEast;

        //East
        else if(x == 1 && z == 0) return EntityDirection.East;

        //South East
        else if(x == 1 && z == -1) return EntityDirection.SouthEast;

        //South
        else if(x == 0 && z == -1) return EntityDirection.South;

        //South West
        else if(x == -1 && z == -1) return EntityDirection.SouthWest;

        //West
        else if(x == -1 && z == 0) return EntityDirection.West;

        //North West
        else if(x == -1 && z == 1) return EntityDirection.NorthWest;

        else return EntityDirection.None;
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
        rigidBody.AddForce(dash.delayedForce, ForceMode.Impulse);
    }

    private void ResetDash() {
        dash.dashing = false;
    }

    private void Cooldown() {
        if(dash.dashCDTimer > 0) dash.dashCDTimer -= Time.deltaTime;
    }
}

