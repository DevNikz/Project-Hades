using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance;

    //Strafe
    public float Horizontal { get; private set; }

    public float Vertical { get; private set; }

    public Vector3 MoveInput {get; private set; }

    //Dash
    public bool DashInput { get; private set; }

    //Input
    private PlayerInput _playerInput;
    private InputAction _Horizontal;
    private InputAction _Vertical;
    private InputAction _Dash;
    
    private void Awake() {
        if (Instance == null) Instance = this;

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
    }

    private void UpdateInputs() {
        //Isometric Move
        Horizontal = _Horizontal.ReadValue<float>();
        Vertical = _Vertical.ReadValue<float>();
        MoveInput = new Vector3(Horizontal, 0f, Vertical);

        //Dash
        DashInput = _Dash.WasPressedThisFrame(); 

        //Debug
        //Debug.Log("Horizontal: " + _Horizontal.activeControl + " | Vertical: " + _Vertical.activeControl);
        //Debug.Log("X: " + MoveInput.x);
        //Debug.Log("Z: " + MoveInput.z);
    }
}
