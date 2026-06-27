using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class PlayerCollider
{
    [Tooltip("Set SphereCollider for Reference")]
    [SerializeField] public SphereCollider collider;

    [Tooltip("Set Radius of the Collider. (Default = 4)")]
    [SerializeField] public float radiusModifier = 4f;
}
