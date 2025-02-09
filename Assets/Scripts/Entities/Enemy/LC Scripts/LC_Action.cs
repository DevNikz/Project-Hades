using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LC_Actions : EnemyAction
{
    private int _comboNum = 0;
    private float _dash = 20;
    private float _dashStr = 50;

    protected override void ProcessAILogic(){
        
    }

    protected override void Attack()
    {
        if (!IsAttacking) Agent.isStopped = false;
        Agent.destination = Player.transform.position;

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            gameObject.transform.LookAt(Player.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        }

        if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < Agent.stoppingDistance)
        {
            _comboNum = 3;
            IsAttacking = true;
            Agent.isStopped = true;

            Attacking();
            Invoke("Attacking", AttackRate);
            Invoke("Attacking", AttackRate * 2);
        }
    }

    protected override void Attacking()
    {
        this.transform.LookAt(Player.transform.position);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

        SetAttackDirection();
        this._attackHitbox.transform.position = this.transform.position + this.transform.forward;
        this._attackHitbox.SetActive(true);
        
        _comboNum++;

        if (_comboNum == 6)
            _rgBody.AddForce(this.transform.forward * _dashStr, ForceMode.Impulse);
        else
            _rgBody.AddForce(this.transform.forward * _dash, ForceMode.Impulse);

        this.SetAction(_comboNum);

        if (_comboNum == 6)
            Invoke("StopAttack", 1f);
        else
            Invoke("StopAttack", 0.3f);
    }

    private void StopAttack()
    {
        this._attackHitbox.SetActive(false);

        if (_comboNum == 6) {
            IsAttacking = false;
            Cooldown = _maxCooldown;
        }
    }

}
