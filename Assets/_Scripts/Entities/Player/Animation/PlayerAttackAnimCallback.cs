using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimCallback : MonoBehaviour
{
    [SerializeField] public bool _isAttacking = false;
    [SerializeField] public bool _isInvulnerable = false;
    [SerializeField] public float _attackMoveSpeed = -1.0f;
}
