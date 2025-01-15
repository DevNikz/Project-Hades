using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Cameras;

public class OHActions : EnemyAction
{
    [SerializeField] private float _speedMultiplier;
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
        Agent.destination = Player.transform.position;
            Agent.speed = _fastSpeed;

            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                gameObject.transform.LookAt(Player.transform.position);
                this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            }

            if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < 2)
            {
                IsAttacking = true;
                this.SetAction(3);
                this._attackHitbox.SetActive(true);
                Agent.isStopped = true;
                Invoke("Attacking", AttackRate);
            }
    }

    protected override void Attacking()
    {
        if (IsAttacking && this.tag == "Enemy")
        {
            this._attackHitbox.SetActive(false);
            this.SetAction(0);
            IsAttacking = false;
            Agent.isStopped = false;
        }
    }
}
