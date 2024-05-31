using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    //Input
    private PlayerInput _playerInput;
    private InputAction _Horizontal;
    private InputAction _Vertical;
    private InputAction _Dash;

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
        //_KeyboardMove = _playerInput.actions["KeyboardMove"];
    }

    private void UpdateInputs() {
        //Isometric Move
        Horizontal = _Horizontal.ReadValue<float>();
        Vertical = _Vertical.ReadValue<float>();
        MoveInput = new Vector3(Horizontal, 0f, Vertical);

        //Dash
        DashInput = _Dash.WasPressedThisFrame(); 

        //Bool Input Move
        // MovePressed = _KeyboardMove.WasPressedThisFrame();
        // MoveHeld = _KeyboardMove.IsPressed();
        // MoveReleased = _KeyboardMove.WasReleasedThisFrame();

        Parameters parameters = new Parameters();
        parameters.PutExtra(PlayerMovement.KEY_MOVE, MoveInput);
        parameters.PutExtra(PlayerMovement.KEY_DASH, DashInput);
        parameters.PutExtra(PlayerMovement.KEY_MOVE_HELD, MoveHeld);

        EventBroadcaster.Instance.PostEvent(EventNames.KeyboardInput.KEY_INPUTS, parameters);

        //Debug.Log(MoveInput);
        
        //Debug
        //Debug.Log("Horizontal: " + _Horizontal.activeControl + " | Vertical: " + _Vertical.activeControl);
        //Debug.Log("X: " + MoveInput.x);
        //Debug.Log("Z: " + MoveInput.z);
    }
}
