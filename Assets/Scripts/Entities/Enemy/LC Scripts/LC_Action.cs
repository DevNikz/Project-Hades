using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LC_Actions : EnemyAction
{
    [NonSerialized] private int _comboNum = 0;
    [SerializeField] private float _dash = 20;
    [SerializeField] private float _dashStr = 50;
    [SerializeField] private float _maxCooldown = 3;

    protected override void ProcessAILogic(){
        
    }

    protected override void Attack()
    {
        if(!Agent.isStopped) Agent.destination = Player.transform.position;

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            gameObject.transform.LookAt(Player.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        }

        if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < 2)
        {
            _comboNum = 3;
            IsAttacking = true;
            Agent.isStopped = true;
            this.SetAction(_comboNum);

            Invoke("Attacking", AttackRate);
            Invoke("Attacking", AttackRate * 2);
            Invoke("Attacking", AttackRate * 3);
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
        {
            _rgBody.AddForce(this.transform.forward * _dash, ForceMode.Impulse);
            this.SetAction(_comboNum);
        }

        Invoke("StopAttack", 0.3f);
    }

    private void StopAttack()
    {
        this._attackHitbox.SetActive(false);

        if (_comboNum == 6) {
            this.SetAction(1);
            IsAttacking = false;
            Cooldown = _maxCooldown;
        }
    }

}
