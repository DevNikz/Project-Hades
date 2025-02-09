using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static EventNames;

public class MS_Actions : EnemyAction
{
    private float _speedMultiplier = 2;
    private float _fastSpeed;
    private float _originalSpeed;

    protected override void BonusOnEnable()
    {
        _fastSpeed = Agent.speed * _speedMultiplier;
        _originalSpeed = Agent.speed;
    }
    protected override void ProcessAILogic()
    {

        if (Action != 1) Agent.speed = _originalSpeed;
    }

    protected override void Attack()
    {
        Agent.isStopped = false;
        Agent.speed = _fastSpeed;

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            gameObject.transform.LookAt(Player.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            Agent.destination = Player.transform.position;
            IsAttacking = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < Agent.stoppingDistance)
        {
            Attacking();
        }
    }

    protected override void Attacking()
    {
        IsAttacking = true;
        this._attackHitbox.SetActive(true);
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        Invoke(nameof(StopAttack), .35f);

        Vector3 randomPoint = this.transform.position + this.transform.forward * _wanderRange;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            gameObject.transform.LookAt(hit.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            Agent.SetDestination(hit.position);
        }
    }

    private void StopAttack()
    {
        this._attackHitbox.SetActive(false);
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
