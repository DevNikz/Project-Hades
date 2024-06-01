using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class InteractList
{
    [Header("Interactables")]
    [SerializeField] public List<Collider> interactList;
}
