using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class Dash
{
    [Tooltip("Set Dash Speed in Float")]
    [SerializeField] public float dashSpeed;
    [Tooltip("Is Entity Dashing?")]
    [SerializeField] public bool dashing;
    [Tooltip("Amount of Dash Force")]
    [SerializeField] public float dashForce;
    [Tooltip("How Long The Dash will last")]
    [SerializeField] public float dashDuration;
    [Tooltip("Cooldown For Dash")]
    [SerializeField] public float dashCD;
    public float dashCDTimer;
    public Vector3 delayedForce;

    public Vector3 isoInput;
}
