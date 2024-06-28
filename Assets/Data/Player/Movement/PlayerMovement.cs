using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovement", menuName = "ProjectHades/Player/Movement", order = 1)]
[InlineEditor]
public class PlayerMovement : ScriptableObject
{
    //Strafe
    [Title("Strafe")]

    [Tooltip("Set Strafing Speed in Float. (Default = 5)")]
    [SerializeField] public float strafeSpeed = 5;

    [Tooltip("Set Current Speed in Float. (Default = strafeSpeed)")]
    [ReadOnly] public float currentSpeed = 5;

    [Tooltip("Set TurnSpeed in Float. (Default = 720)")]
    [SerializeField] public float turnSpeed = 720;

    [Tooltip("Set GroundDrag in Float. (Default = 10)")]
    [SerializeField] public float groundDrag = 10;

    //Dash
    [Title("Dash")]
    [Tooltip("Set Dash Speed in Float. (Default = 10)")]
    [SerializeField] public float dashSpeed = 10;

    [Tooltip("Amount of Dash Force. (Default = 25)")]
    [SerializeField] public float dashForce = 25;

    [Tooltip("How Long The Dash will last. (Default = 0.025)")]
    [SerializeField] public float dashDuration = 0.025f;

    [Tooltip("Cooldown For Dash. (Default = 1.5)")]
    [SerializeField] public float dashCD = 1.5f;

    [Tooltip("Is Entity Dashing?")]
    [ReadOnly] public bool dashing;
    [ReadOnly] public float dashCDTimer;
    [ReadOnly] public Vector3 delayedForce;
    [ReadOnly] public Vector3 isoInput;

    //States
    [Title("States")]
    [ReadOnly] public EntityState state = EntityState.Idle;
    [ReadOnly] public EntityDirection direction = EntityDirection.None;
}

