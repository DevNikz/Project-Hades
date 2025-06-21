using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LC_Actions : EnemyAction
{
    private int _comboNum = 4;
    private float _dash = 50;
    private float _dashStr = 75;

    protected override void ProcessAILogic(){
        if (Cooldown > _timerDelay) _comboNum = 4;
    }

    protected override void Attack()
    {
        IsAttacking = false;
        Agent.isStopped = false;
        Agent.SetDestination(Player.transform.position + Calculate());

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            gameObject.transform.LookAt(Player.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        }

        if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < Agent.stoppingDistance)
        {
            IsAttacking = true;
            Agent.isStopped = true;

            Attacking();
        }
    }

    protected override void Attacking()
    {
        this.transform.LookAt(Player.transform.position);

        if (_comboNum > 6) _comboNum = 6;
        if (_comboNum < 4) _comboNum = 4;

        this.SetAction(_comboNum);    
    }

    public void StopAttack()
    {
        IsAttacking = false;
        Cooldown = AttackRate;
        TeleportPoint();
    }

    public void TurnHitOn()
    {
        this.transform.LookAt(Player.transform.position + Calculate());
        SetAttackDirection();
        this._attackHitbox.transform.position = this.transform.position + this.transform.forward;
        this._attackHitbox.SetActive(true);

        if (_comboNum == 6)
            _rgBody.AddForce(this.transform.forward * _dashStr, ForceMode.Impulse);
        else
            _rgBody.AddForce(this.transform.forward * _dash, ForceMode.Impulse);

        _comboNum++;
        if (_comboNum > 6) _comboNum = 6;
        if (_comboNum < 4) _comboNum = 4;
    }

    public void TurnHitOff()
    {
        this._attackHitbox.SetActive(false);
    }

    public void BeginAttack()
    {
        Attacking();
    }

}
