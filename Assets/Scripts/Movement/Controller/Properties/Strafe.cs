using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable] public class Strafe
{
    [Tooltip("Set Current Speed in Float")]
    [SerializeField] public float currentSpeed;

    [Tooltip("Set Strafing Speed in Float")]
    [SerializeField] public float strafeSpeed;


    [Tooltip("Set TurnSpeed in Float")]
    [SerializeField] public float turnSpeed;
    [Tooltip("Set GroundDrag in Float")]
    [SerializeField] public float groundDrag;
    
}
