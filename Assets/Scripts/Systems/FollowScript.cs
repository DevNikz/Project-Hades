using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    [SerializeField] private GameObject parentRef;
    void Update()
    {
        this.transform.position = parentRef.transform.position;
    }
}
