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
    private bool isActionFinished = true;
    private bool isRepeated = false;
    private bool isReaping = false;
    private int count = 0;
    [SerializeField] private float reapDistance = 5f;
    [SerializeField] private float chargeDistance = 5f;
    [SerializeField] private float chargeCooldown = 0.25f;
    [SerializeField] private float actionCooldown = 2f;
    private float currentCooldown;
    [SerializeField] private GameObject circleHitBox;

    protected override void BonusOnEnable()
    {
        _fastSpeed = Agent.speed * _speedMultiplier;
        _originalSpeed = Agent.speed;

        currentCooldown = actionCooldown;
    }
    protected override void ProcessAILogic()
    {

        if (Action != 1) Agent.speed = _originalSpeed;

        if (Action == 3) Reap();
        if (Action == 4) Dash();
    }

    protected override void Attack()
    {
        if (isActionFinished)
        {
            count = 0;
            float percentHP = this.GetComponent<EnemyController>().getPercentHP();
            currentCooldown = actionCooldown * percentHP;
            if (currentCooldown <= 0.25f) currentCooldown = 0.05f;

            if (percentHP > 0.65f)
                this.SetAction(3);

            else if (percentHP > 0.25f)
            {
                if (Random.Range(0f, 1f) <= percentHP)
                    this.SetAction(3);
                else
                    Dash();
            }

            else
                Dash();

            isActionFinished = false;
        }
        else
            Dash();
    }

    protected override void Attacking() { }

    private void Reap()
    {
        if (!isReaping)
        {
            Agent.isStopped = false;
            Agent.SetDestination(Player.transform.position);
        }

        if (!isReaping && Vector3.Distance(this.transform.position, Player.transform.position) < reapDistance)
        {
            isReaping = true;
            Agent.isStopped = true;
            circleHitBox.SetActive(true);
        }

        if (isReaping)
            Reaping();

        if (circleHitBox.transform.localScale.x >= 15 || (isReaping && !circleHitBox.activeSelf))
        {
            FinishAction();
            isReaping = false;
            Agent.isStopped = false;
            circleHitBox.SetActive(false);
            circleHitBox.transform.localScale = new Vector3(5, 2, 5);
        }
    }

    private void Reaping()
    {
        circleHitBox.transform.localScale += new Vector3(0.15f, 0, 0.15f);
        Debug.Log("count");
    }

    private void Dash()
    {
        Agent.speed = _fastSpeed;

        if (!IsAttacking)
        {
            Agent.SetDestination(Player.transform.position);
            Agent.isStopped = false;
        }

        if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < chargeDistance)
            Dashing();

        if (IsAttacking && Agent.remainingDistance <= Agent.stoppingDistance && !isRepeated)
        {
            Invoke(nameof(StopDash), chargeCooldown);
            this._attackHitbox.SetActive(false);
            Agent.isStopped = true;
            isRepeated = true;
            count++;

            this.SetAction(1);
        }

        if (count >= 3) FinishAction();
    }

    protected void Dashing()
    {
        IsAttacking = true;
        this.SetAction(4);

        this._attackHitbox.SetActive(true);
        //this.gameObject.GetComponent<BoxCollider>().enabled = false;

        gameObject.transform.LookAt(Player.transform.position);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

        findPoint(10f);
    }

    private void StopDash()
    {
        IsAttacking = false;
        isRepeated = false;
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

    private void FinishAction()
    {
        isActionFinished = true;
        this.Cooldown = currentCooldown;
    }

    public override void SetHit(AttackDirection attackDirection)
    {
        /*
        EndAttack();
        if (Cooldown < anims.timer) Cooldown = anims.timer;

        //Cronos Boss Function
        isActionFinish = true;
        isReaping = false;
        Agent.isStopped = false;
        circleHitBox.SetActive(false);
        circleHitBox.transform.localScale = new Vector3(5, 2, 5);

        anims.SetHit(attackDirection);
        Invoke(nameof(ResetHit), anims.timer);
        */
    }
}
