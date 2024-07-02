using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovement", menuName = "ProjectHades/Player/Movement", order = 1)]
[InlineEditor]
public class PlayerMovement : ScriptableObject
{
    //Strafe
    [Title("Strafe")]

    [Tooltip("Set Strafing Speed in Float. (Default = 5)")]
    public float strafeSpeed;

    [Tooltip("Set TurnSpeed in Float. (Default = 720)")]
    public float turnSpeed;

    [Tooltip("Set GroundDrag in Float. (Default = 10)")]
    public float groundDrag;

    //Dash
    [Title("Dash")]
    [Tooltip("Set Dash Speed in Float. (Default = 10)")]
    public float dashSpeed;

    [Tooltip("Amount of Dash Force. (Default = 25)")]
    [Range(0.1f,100f)] public float dashForce;

    [Tooltip("How Long The Dash will last. (Default = 0.025)")]
    public float dashDuration;

    [Tooltip("Cooldown For Dash. (Default = 1.5)")]
    public float dashCD;
}

