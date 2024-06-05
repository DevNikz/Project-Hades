using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UserInput : MonoBehaviour
{
    //Strafe
    public float Horizontal { get; private set; }

    public float Vertical { get; private set; }

    public Vector3 MoveInput { get; private set; }

    public bool MovePressed { get; private set;}
    
    public bool MoveHeld { get; private set; }

    public bool MoveReleased { get; private set; }

    //Dash
    public bool DashInput { get; private set; }

    //Interact
    public bool interactPress { get; private set; }
    public bool interactHeld { get; private set; }
    public bool interactReleased { get; private set; }

    //Input
    private PlayerInput _playerInput;
    private InputAction _Horizontal;
    private InputAction _Vertical;
    private InputAction _Dash;
    private InputAction _Interact;

    //Bool Ref
    private InputAction _KeyboardMove;
    
    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }

    private void Update() {
        UpdateInputs();
    }

    private void SetupInputActions() {
        _Horizontal = _playerInput.actions["HorizontalMove"];
        _Vertical = _playerInput.actions["VerticalMove"];
        _Dash = _playerInput.actions["Dash"];
        _Interact = _playerInput.actions["Interact"];
    }

    private void UpdateInputs() {
        //Isometric Move
        Horizontal = _Horizontal.ReadValue<float>();
        Vertical = _Vertical.ReadValue<float>();
        MoveInput = new Vector3(Horizontal, 0f, Vertical);

        //Dash
        DashInput = _Dash.WasPressedThisFrame(); 

        //Interact
        interactPress = _Interact.WasPressedThisFrame();
        interactHeld = _Interact.IsPressed();
        interactReleased = _Interact.WasReleasedThisFrame();

        Parameters parameters = new Parameters();
        parameters.PutExtra(Movement.KEY_MOVE, MoveInput);
        parameters.PutExtra(Movement.KEY_DASH, DashInput);
        EventBroadcaster.Instance.PostEvent(EventNames.KeyboardInput.KEY_INPUTS, parameters);

        parameters = new Parameters();
        parameters.PutExtra(ColliderModule.INPUT_PRESS, interactPress);
        EventBroadcaster.Instance.PostEvent(EventNames.KeyboardInput.INTERACT_PRESS, parameters);

        parameters = new Parameters();
        parameters.PutExtra(ColliderModule.INPUT_HOLD, interactHeld);
        EventBroadcaster.Instance.PostEvent(EventNames.KeyboardInput.INTERACT_HOLD, parameters);

        parameters = new Parameters();
        parameters.PutExtra(ColliderModule.INPUT_TOGGLE, interactPress);
        EventBroadcaster.Instance.PostEvent(EventNames.KeyboardInput.INTERACT_TOGGLE, parameters);
    }
}
