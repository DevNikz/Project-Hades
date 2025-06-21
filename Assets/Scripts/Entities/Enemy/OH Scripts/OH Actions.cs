using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Cameras;

public class OHActions : EnemyAction
{
    private float _speedMultiplier = 3;
    private float _fastSpeed;
    private float _originalSpeed;

    protected override void BonusOnEnable()
    {
        _fastSpeed = Agent.speed * _speedMultiplier;
        _originalSpeed = Agent.speed;
    }
    protected override void ProcessAILogic(){
        
        if (Action != 1) Agent.speed = _originalSpeed;
    }

    protected override void Attack()
    {
        Agent.isStopped = false;
        Agent.SetDestination(Player.transform.position + Calculate());
        Agent.speed = _fastSpeed;

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            gameObject.transform.LookAt(Player.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        }        

        if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < Agent.stoppingDistance)
        {
            IsAttacking = true;
            Agent.isStopped = true;
            this.SetAction(3);
            //Invoke(nameof(Attacking), 0.75f);
        }
    }

    protected override void Attacking()
    {
        /*
        this._attackHitbox.SetActive(true);
        Agent.isStopped = true;
        Invoke(nameof(StopAttack), AttackRate);
        */
    }

    public void BeginAttack()
    {
        gameObject.transform.LookAt(Player.transform.position);
        this._attackHitbox.SetActive(true);
    }

    public void StopAttack()
    {
        this._attackHitbox.SetActive(false);
        IsAttacking = false;
        Cooldown = AttackRate;
    }
}
