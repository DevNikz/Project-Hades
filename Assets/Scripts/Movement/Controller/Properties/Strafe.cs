using System;
using Unity.Collections;
using UnityEngine;

[Serializable] public class Strafe
{
    [Tooltip("Set Current Speed in Float. (Default = strafeSpeed)")]
    [ReadOnly] public float currentSpeed = 5;

    [Tooltip("Set Strafing Speed in Float. (Default = 5)")]
    [SerializeField] public float strafeSpeed = 5;


    [Tooltip("Set TurnSpeed in Float. (Default = 720)")]
    [SerializeField] public float turnSpeed = 720;

    [Tooltip("Set GroundDrag in Float. (Default = 10)")]
    [SerializeField] public float groundDrag = 10;
    
}
