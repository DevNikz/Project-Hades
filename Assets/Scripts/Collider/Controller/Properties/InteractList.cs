using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class InteractList
{
    [Header("Interactables")]

    [SerializeField] public List<Collider> eventList;
    [SerializeField] public List<Collider> pressList;
    [SerializeField] public List<Collider> holdList;
    [SerializeField] public List<Collider> toggleList;
}
