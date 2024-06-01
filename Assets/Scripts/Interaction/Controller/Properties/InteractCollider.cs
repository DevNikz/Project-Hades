using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class InteractCollider
{
    [Tooltip("Set SphereCollider for Reference")]
    [SerializeField] public SphereCollider collider;

    [Tooltip("Set Radius of the Collider. (Default = 3)")]
    [SerializeField] public float radiusModifier = 3f;
}
