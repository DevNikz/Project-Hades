using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    //Properties
    [Header("Properties")]
    [Tooltip("Set Rigidbody reference of the GameObject")]
    [SerializeField] private Rigidbody rigidBody;
    [Tooltip("Set Transform property reference of the GameObject")]
    [SerializeField] private Transform model;
    private Vector3 input;

    [Header("Speed")]
    [Tooltip("Set Current Speed in Float")]
    [SerializeField] private float currentSpeed;
    [Tooltip("Set Strafing Speed in Float")]
    [SerializeField] private float strafeSpeed = 5f;
    [Tooltip("Set Dash Speed in Float")]
    [SerializeField] private float dashSpeed = 5f;
    [Tooltip("Set TurnSpeed in Float")]
    [SerializeField] private float turnSpeed = 360f;
    [Tooltip("Set GroundDrag in Float")]
    [SerializeField] private float groundDrag = 4f;
    [Tooltip("Current Movement State of the Object")]

    [Header("State")]
    [SerializeField] public Movement movement = Movement.Strafing;
    

    //Dashing
    [Header("Dash")]
    [SerializeField] public bool dashing;
    [SerializeField] public float dashForce = 5f;
    [SerializeField] public float dashDuration = 0.25f;
    [SerializeField] public float dashCD = 1.5f;
    private float dashCDTimer;

    //Keybinds
    [Header("Keybinds")]
    [SerializeField] public KeyCode Up = KeyCode.W;
    [SerializeField] public KeyCode Down = KeyCode.S;
    [SerializeField] public KeyCode Left = KeyCode.A;
    [SerializeField] public KeyCode Right = KeyCode.D;
    [SerializeField] public KeyCode dashKey = KeyCode.LeftShift;
    

    private void Update() {
        GatherInput();
        Look();
        Actions();
        CheckDrag();
        Cooldown();
    }

    private void FixedUpdate() {
        StateHandler();
        Move();
    }

    private void GatherInput() {
        input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
    }

    private void Look() {
        if(input == Vector3.zero) return;

        //var rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        Quaternion rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rot, turnSpeed * Time.deltaTime);
    }

    private void StateHandler() {
        if(Input.GetKey(dashKey)) {
            movement = Movement.Dashing;
            currentSpeed = dashSpeed;
        }

        else if(Input.GetKey(Up) || Input.GetKey(Down) || Input.GetKey(Left) || Input.GetKey(Right)) {
            movement = Movement.Strafing;
            currentSpeed = strafeSpeed;
        }
    }

    private void Move() {
        //_rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
        rigidBody.MovePosition(transform.position + input.ToIso() * input.normalized.magnitude * strafeSpeed * Time.deltaTime);
    }

    private void Dash() {
        if(dashCDTimer > 0 ) return;
        else dashCDTimer = dashCD;

        this.dashing = true;
        Vector3 forceToApply = gameObject.transform.forward * dashForce;
        delayedForce = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForce;
    private void DelayedDashForce() {
        rigidBody.AddForce(delayedForce, ForceMode.Impulse);
    }

    private void ResetDash() {
        this.dashing = false;
    }

    private void Actions() {
         if(Input.GetKeyDown(this.dashKey)) this.Dash();
    }

    private void CheckDrag() {
        if(movement == Movement.Strafing) {
            rigidBody.drag = groundDrag;
        }
        else rigidBody.drag = 0f;
    }

    private void Cooldown() {
        if(dashCDTimer > 0) dashCDTimer -= Time.deltaTime;
        Debug.Log(dashCDTimer);
    }

    // private IEnumerator playerDash(float secs) {
        
    //     rigidBody.AddForce(rigidBody.gameObject.transform.forward * 10f);

    //     yield return new WaitForSeconds(secs);
    // }

    // private IEnumerator debugCooldown(float secs) {
    //     this.speed = 20;
    //     yield return new WaitForSeconds(secs);
    // }

    // private void Actions() {
    //     if(Input.GetKeyDown(KeyCode.LeftShift))
    //     {
    //         this.isDash = true;
    //     }
    // }

    // private void ActionsChecker() {
    //     if(this.isDash == true) {
    //         this.isDash = false;
    //         Debug.Log("Test");
    //         this.StartCoroutine(debugCooldown(2.5f));
    //     }
    //     else if(this.isDash == false) {
    //         this.speed = 5;
    //     }
    // }
}

