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

    //Dash
    public bool DashInput { get; private set; }

    //Interact
    public bool interactPress { get; private set; }
    public bool interactHeld { get; private set; }
    public bool interactReleased { get; private set; }

    //Position
    public Vector2 mousePosition { get; private set; }

    //Input
    private PlayerInput _playerInput;
    private InputAction _Horizontal;
    private InputAction _Vertical;
    private InputAction _Dash;
    private InputAction _Interact;
    private InputAction _MousePosition;
    
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
        _MousePosition = _playerInput.actions["MousePosition"];
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

        //MousePosition
        mousePosition = _MousePosition.ReadValue<Vector2>();
        Vector3 projectedPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //Move
        Broadcaster.Instance.AddVectorBParam(Movement.KEY_MOVE, Movement.KEY_DASH, 
                                            EventNames.KeyboardInput.KEY_INPUTS, MoveInput, DashInput);
        
        //Interact
        Broadcaster.Instance.AddBoolParam(ColliderModule.INPUT_PRESS, EventNames.KeyboardInput.INTERACT_PRESS, interactPress);
        Broadcaster.Instance.AddBoolParam(ColliderModule.INPUT_HOLD, EventNames.KeyboardInput.INTERACT_HOLD, interactHeld);
        Broadcaster.Instance.AddBoolParam(ColliderModule.INPUT_TOGGLE, EventNames.KeyboardInput.INTERACT_TOGGLE, interactPress);

        //MousePosition
        Broadcaster.Instance.AddVectorParam(MouseMoveTest.MOUSE_POS, EventNames.MouseInput.MOUSE_POS, projectedPosition);
    }
}
