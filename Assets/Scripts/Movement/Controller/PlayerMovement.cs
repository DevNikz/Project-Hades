using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    /*
    - Transfer EventManager
    - Separate Strafe and Dash
    */
    
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
    [SerializeField] public Movement movement = Movement.Strafing;
    public Direction direction;

    //Dashing
    [Header("Dash")]

    [SerializeField] private Dash dash;

    //Keybinds
    [Header("Keybinds")]
    [SerializeField] private string debugString = "W.I.P";

    //Input References
    private Vector3 moveInput;
    private bool dashInput;


    //Will Fix This Rebinds soon
    /*
    [SerializeField] public KeyCode Up = KeyCode.W;
    [SerializeField] public KeyCode Down = KeyCode.S;
    [SerializeField] public KeyCode Left = KeyCode.A;
    [SerializeField] public KeyCode Right = KeyCode.D;
    [SerializeField] public KeyCode dashKey = KeyCode.LeftShift;
    */

    /*
    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable() {
        move.Disable();
    }
    */

    private void Update() {

        //Init Input From UserInput
        GatherInput();

        //Init Strafe Funcs
        Look();
        CheckDrag();

        //Init Dash Funcs
        Cooldown();
    }

    private void FixedUpdate() {
        StateHandler();
        Move();
    }

    private void GatherInput() {
        //Old System
        //input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));

        //New System
        //Horizontal = move.ReadValue<float>();
        //Vertical = move.ReadValue<float>();

        //Grabs instance of MoveInput
        moveInput = UserInput.Instance.MoveInput;
        dashInput = UserInput.Instance.DashInput;
    }

    private void Look() {
        if(moveInput == Vector3.zero) return;

        //var rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        Quaternion rot = Quaternion.LookRotation(moveInput.ToIso(), Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rot, strafe.turnSpeed * Time.deltaTime);
    }

    private void StateHandler() {
        if(UserInput.Instance.DashInput) {
            movement = Movement.Dashing;
            strafe.currentSpeed = dash.dashSpeed;
            Dash();
        }

        else if(UserInput.Instance.Horizontal != 0 || UserInput.Instance.Vertical != 0) {
            movement = Movement.Strafing;
            strafe.currentSpeed = strafe.strafeSpeed;

            //Debug Direction
            direction = IsoCompass(UserInput.Instance.Vertical, UserInput.Instance.Horizontal);
            //Debug.Log("X: " + UserInput.Instance.Vertical + " | Z: " + UserInput.Instance.Horizontal);
            //Debug.Log(direction);
        }
    }

    private Direction IsoCompass(float x, float z) {
        //North
        if(x == 1 && z == 0) return Direction.North;

        //North East
        else if(x == 1 && z == 1) return Direction.NorthEast;

        //East
        else if(x == 0 && z == 1) return Direction.East;

        //South East
        else if(x == -1 && z == 1) return Direction.SouthEast;

        //South
        else if(x == -1 && z == 0) return Direction.South;

        //South West
        else if(x == -1 && z == -1) return Direction.SouthWest;

        //West
        else if(x == 0 && z == -1) return Direction.West;

        //North West
        else if(x == 1 && z == -1) return Direction.NorthWest;

        else return Direction.None;
    }

    private void Move() {
        //_rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
        rigidBody.MovePosition(transform.position + moveInput.ToIso() * moveInput.normalized.magnitude * strafe.strafeSpeed * Time.deltaTime);
    }

    private void CheckDrag() {
        if(movement == Movement.Strafing) {
            rigidBody.drag = strafe.groundDrag;
        }
        else rigidBody.drag = 5f;
    }

    private void Dash() {
        if(dash.dashCDTimer > 0 ) return;
        else dash.dashCDTimer = dash.dashCD;
        dash.dashing = true;

        //Dashes Based on Front Face
        //Vector3 forceToApply = gameObject.transform.forward * dashForce;

        //Dash Based on Key Input
        //Vector3 forceToApply = moveInput * dashForce; //World View

        dash.isoInput = this.ConvertToIso(moveInput.z, moveInput.x);

        Vector3 forceToApply = dash.isoInput * dash.dashForce; 

        //Apply Force
        dash.delayedForce = forceToApply;

        //Duration
        Invoke(nameof(DelayedDashForce), 0.025f);

        //Cooldown
        Invoke(nameof(ResetDash), dash.dashDuration);
    }

    private Vector3 ConvertToIso(float x, float z) {

        Debug.Log("X: " + x + " Z: " + z);
        //North
        if(x == 1 && z == 0) return new Vector3(1f, 0f, 1f);

        //North East
        else if(x == 1 && z == 1) return new Vector3(1f, 0f, 0f);

        //East
        else if(x == 0 && z == 1) return new Vector3(1f, 0f, -1f);

        //South East
        else if(x == -1 && z == 1) return new Vector3(0f, 0f, -1f);

        //South
        else if(x == -1 && z == 0) return new Vector3(-1f, 0f, -1f);

        //South West
        else if(x == -1 && z == -1) return new Vector3(-1f, 0f, 0f);

        //West
        else if(x == 0 && z == -1) return new Vector3(-1f, 0f, 1f);

        //North West
        else if(x == 1 && z == -1) return new Vector3(0f, 0f, 1f);

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
        //Debug.Log(dashCDTimer);
    }
}

