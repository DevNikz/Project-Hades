using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventNames;
using UnityEngine.AI;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;

public class CronosAI : EnemyAction
{
    private float _speedMultiplier = 3;
    private float _fastSpeed;
    private float _originalSpeed;
    private bool  isCharging = false;
    private bool  isActionFinish = false;
    private bool  repeater = false;
    private int   count = 0;

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
        Dash();
        /*
        if (isActionFinish) 
        {
            float percentHP = this.GetComponent<EnemyController>().getPercentHP();
            if (percentHP > 0.65f)
            {
                Reap();
            }
            else if (percentHP > 0.25f)
            {
                if (Random.Range(0f, 1f) > percentHP) Dash();
                else Reap();
            }
            else
                Dash();
        }
        */
    }

    private void Reap()
    {

    }

    private void Dash()
    {
        /*
        if (count >= 3)
        {
            Cooldown = 3;
            count = 0;
            isCharging = false;
        }
        */

        Agent.isStopped = false;
        Agent.speed = _fastSpeed;
        //if(isCharging) _rgBody.AddForce(this.transform.forward * 3, ForceMode.Impulse);

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            Agent.isStopped = true;

            if (!repeater)
            {
                StartCoroutine(charge());
                repeater = true;
            }
            

            /*
            if (!isCharging)
            {
                Agent.SetDestination(Player.transform.position);
                gameObject.transform.LookAt(Player.transform.position);
                //this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            }
            else findPoint();
            isCharging = !isCharging;
            */
        }

        if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < Agent.stoppingDistance)
        {
            Attacking();
        }
    }

    private IEnumerator charge()
    {
        yield return new WaitForSeconds(1);
        Agent.isStopped = false;

        IsAttacking = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.transform.LookAt(Player.transform.position);
        findPoint();

        isCharging = true;
        repeater = false;
        count++;
    }

    protected override void Attacking()
    {
        IsAttacking = true;
        this._attackHitbox.SetActive(true);
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        Invoke(nameof(StopAttack), .3f);
        //findPoint();
    }

    private void findPoint()
    {
        Vector3 randomPoint = this.transform.position + this.transform.forward * _wanderRange * 2f;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            gameObject.transform.LookAt(hit.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            Agent.SetDestination(hit.position);
            //StartCoroutine(cutPath());
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
