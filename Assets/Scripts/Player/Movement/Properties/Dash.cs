using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable] public class Dash
{
    [Tooltip("Set Dash Speed in Float. (Default = 10)")]
    [SerializeField] public float dashSpeed = 10;

    [Tooltip("Is Entity Dashing?")]
    [ReadOnly] public bool dashing;

    [Tooltip("Amount of Dash Force. (Default = 25)")]
    [SerializeField] public float dashForce = 25;

    [Tooltip("How Long The Dash will last. (Default = 0.025)")]
    [SerializeField] public float dashDuration = 0.025f;

    [Tooltip("Cooldown For Dash. (Default = 1.5)")]
    [SerializeField] public float dashCD = 1.5f;

    [ReadOnly] public float dashCDTimer;
    [ReadOnly] public Vector3 delayedForce;
    [ReadOnly] public Vector3 isoInput;
}
