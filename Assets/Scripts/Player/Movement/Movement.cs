using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    [Space] [Title("Movement")]
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
    [ReadOnly] [SerializeReference] private bool dashInput;

    //Broadcaster
    public const string KEY_MOVE = "KEY_MOVE";
    
    public const string KEY_DASH = "KEY_DASH";

    public const string KEY_MOVE_HELD = "KEY_MOVE_HELD";
    public const string RIGHT_CLICK = "RIGHT_CLICK";

    void Reset() {
        rigidBody = this.GetComponent<Rigidbody>();
        model = this.GetComponent<Transform>();

        movement.state = EntityState.Idle;
        movement.direction = EntityDirection.None;

        movement.currentSpeed = 5;
        movement.strafeSpeed = 5;
        movement.turnSpeed = 720;
        movement.groundDrag = 10;

        movement.dashSpeed = 10;
        movement.dashing = false;
        movement.dashForce = 25;
        movement.dashDuration = 0.025f;
        movement.dashCD = 1.5f;
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
        
        if(dashInput || PlayerData.isAttacking) return;
        else {
            rigidBody.MovePosition(transform.position + moveInput.ToIso() * moveInput.normalized.magnitude * movement.currentSpeed * Time.deltaTime);
        }
    }

    private void CheckMove() {
        ParticleSystem.EmissionModule temp = dust.emission;
        if(movement.state == EntityState.Strafing) temp.enabled = true;
        else temp.enabled = false;
    }

    private void CheckDash() {
        if(movement.state == EntityState.Dashing) dashParticle.Play();
    }

    private void lookEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);

        if(moveInput == Vector3.zero) return;

        // Quaternion rot = Quaternion.LookRotation(moveInput.ToIso(), Vector3.up);
        // model.rotation = Quaternion.RotateTowards(model.rotation, rot, strafe.turnSpeed * Time.deltaTime);
    }

    private void stateHandlerEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);
        dashInput = parameters.GetBoolExtra(KEY_DASH, false);

        if(dashInput == true) {
            movement.currentSpeed = movement.dashSpeed;
            Dash();
        }

        if(moveInput.x != 0 || moveInput.z != 0) {
            //Set To Strafing
            movement.state = EntityState.Strafing;
            PlayerData.entityState = EntityState.Strafing;

            //Set To Strafing Speed
            movement.currentSpeed = movement.strafeSpeed;

            //Debug Direction
            movement.direction = IsoCompass(moveInput.x, moveInput.z);
        }

        if(moveInput.x == 0 && moveInput.z == 0) {
            movement.state = EntityState.Idle;
            PlayerData.entityState = EntityState.Idle;
        }

        if(PlayerData.entityState == EntityState.BasicAttack){
            movement.state = EntityState.BasicAttack;
            PlayerData.entityState = EntityState.BasicAttack;
        }
    }

    private void CheckDrag() {
        if(movement.state == EntityState.Strafing) {
            rigidBody.drag = movement.groundDrag;
        }
        else rigidBody.drag = 10f;
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
        if(movement.dashCDTimer > 0 ) return;
        else movement.dashCDTimer = movement.dashCD;

        //Set Dash To True
        movement.dashing = true;

        //Convert World View Coords To Iso Coords
        movement.isoInput = this.ConvertToIso(moveInput.x, moveInput.z);

        //Apply Dash Based On KeyInput
        Vector3 forceToApply = movement.isoInput * movement.dashForce; 

        //Apply Force
        movement.delayedForce = forceToApply;

        //Duration
        Invoke(nameof(DelayedDashForce), 0.025f);

        //Cooldown
        Invoke(nameof(ResetDash), movement.dashDuration);
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
        movement.state = EntityState.Dashing;
        PlayerData.entityState = EntityState.Dashing;
        rigidBody.AddForce(movement.delayedForce, ForceMode.Impulse);
    }

    private void ResetDash() {
        movement.dashing = false;
    }

    private void Cooldown() {
        if(movement.dashCDTimer > 0) movement.dashCDTimer -= Time.deltaTime;
    }
}

