using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static EventNames;

public class MS_Actions : EnemyAction
{
    private float _speedMultiplier = 3;
    private float _fastSpeed;
    private float _originalSpeed;
    private bool  isCharging = false;

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
            IsAttacking = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;

            if (!isCharging)
            {
                Agent.SetDestination(Player.transform.position);
                gameObject.transform.LookAt(Player.transform.position);
                //this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            }
            else findPoint();
            isCharging = !isCharging;
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
        Invoke(nameof(StopAttack), .3f);
        findPoint();
    }

    private void findPoint()
    {
        Vector3 randomPoint = this.transform.position + this.transform.forward * _wanderRange * 0.75f;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            gameObject.transform.LookAt(hit.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            Agent.SetDestination(hit.position);
            StartCoroutine(cutPath());
        }
    }

    private IEnumerator cutPath()
    {
        yield return new WaitForEndOfFrame();
        Vector3[] pos = Agent.path.corners;
        if (pos.Length < 2) yield break;
        for (int i = 2; i < pos.Length; i++)
        {
            pos[i] = pos[2];
        }
    }

    private void StopAttack()
    {
        this._attackHitbox.SetActive(false);
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
