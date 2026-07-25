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
    [SerializeField] private float chargeDistance = 5;
    private bool  isCharging = false;
    private int count = 0;

    protected override void BonusOnEnable()
    {
        _fastSpeed = Agent.speed * _speedMultiplier;
        _originalSpeed = Agent.speed;
    }
    protected override void ProcessAILogic()
    {

        if (Action != 1) Agent.speed = _originalSpeed;

        if (Action == 3) Attack();
    }

    protected override void Attack()
    {
        Agent.isStopped = false;
        Agent.speed = _fastSpeed;

        if (!IsAttacking)
            Agent.SetDestination(Player.transform.position);

        if (!IsAttacking && Agent.remainingDistance < chargeDistance)
            Attacking();

        if (IsAttacking && Agent.remainingDistance <= Agent.stoppingDistance)
            StopAttack();
    }

    protected override void Attacking()
    {
        IsAttacking = true;
        this.SetAction(3);

        this._attackHitbox.SetActive(true);
        //this.gameObject.GetComponent<BoxCollider>().enabled = false;

        gameObject.transform.LookAt(Player.transform.position);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

        findPoint(10f);
    }

    private void findPoint(float multiplier)
    {
        Vector3 randomPoint = this.transform.position + this.transform.forward * multiplier;
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
        IsAttacking = false;
        this._attackHitbox.SetActive(false);
        //this.gameObject.GetComponent<BoxCollider>().enabled = true;
        this.SetAction(1);
        if (count >= 3)
        {
            count = 0;
            this.Cooldown = this.AttackRate;
        }
        else
        {
            count++;
            //this.Cooldown = 0.2f;
        }
    }

    public override void SetHit(AttackDirection attackDirection)
    {
        Debug.Log("Dmg");
        if (anims.isDead || anims.isStun || anims.isHit || IsAttacking) return;

        EndAttack();
        if (Cooldown < _timerDelay) Cooldown = _timerDelay;

        anims.SetHit(attackDirection);
        //Invoke(nameof(ResetHit), _timerDelay);
    }
}
