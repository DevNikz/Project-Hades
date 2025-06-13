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

    // 0 - Reap
    // 1 - Dash
    private Dictionary<string, Attacks> possibleAttacks = new Dictionary<string, Attacks>();
    private string chosenAttack = "Reap";

    [SerializeField] private float reapDistance = 5f;
    [SerializeField] private float reapDelay = 0.25f;
    [SerializeField] private float reapLength = 15f;
    [SerializeField] private float reapSpeed = 0.3f;
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

        Attacks temp = this.AddComponent<Attacks>();
        possibleAttacks.Add("Reap", temp);
        possibleAttacks["Reap"].distance = reapDistance;

        temp = this.AddComponent<Attacks>();
        possibleAttacks.Add("Dash", temp);
        possibleAttacks["Dash"].distance = chargeDistance;
        possibleAttacks["Dash"].cooldown = chargeCooldown;
    }
    protected override void ProcessAILogic()
    {
        if(Cooldown > 0.25)
        {
            //this._attackHitbox.SetActive(false);
            circleHitBox.SetActive(false);
            circleHitBox.transform.localScale = new Vector3(5, 2, 5);
        }

        if (Action != 1) Agent.speed = _originalSpeed;

        if (Action == 3) Reap();
        if (Action == 4) Dash();
    }

    protected override void Attack()
    {
        float percentHP = this.GetComponent<EnemyController>().getPercentHP();
        if(IsAttacking) Agent.speed = _fastSpeed;

        if (!IsAttacking)
        {
            Agent.SetDestination(Player.transform.position);
            Agent.isStopped = false;
            Agent.speed = _originalSpeed;

            currentCooldown = actionCooldown * percentHP;
            if (currentCooldown <= 0.25f) currentCooldown = 0.25f;
        }

        if (!IsAttacking && Vector3.Distance(this.transform.position, Player.transform.position)
            < possibleAttacks[chosenAttack].distance)
        {
            Invoke(chosenAttack, 0);
        }

        if (isActionFinished)
        {
            count = 0;
            if (percentHP > 0.65f)
                chosenAttack = "Reap";

            else if (percentHP > 0.25f)
            {
                if (Random.Range(0f, 1f) <= percentHP)
                    chosenAttack = "Reap";
                else
                    chosenAttack = "Dash";
            }

            else
                chosenAttack = "Dash";

            isActionFinished = false;
        }
    }

    protected override void Attacking() { }

    private void Reap()
    {
        if (this.Action == 1)
        {
            //Invoke(nameof(BeginAttack), reapDelay);

            Agent.isStopped = true;   
            this.SetAction(3);
        }

        if (IsAttacking)
            Reaping();

        // || (IsAttacking && !circleHitBox.activeSelf)
        if (IsAttacking && circleHitBox.transform.localScale.x >= reapLength)
        {
            FinishAction();
            IsAttacking = false;
            Agent.isStopped = false;
            circleHitBox.SetActive(false);
            circleHitBox.transform.localScale = new Vector3(5, 2, 5);
        }
    }

    private void Reaping()
    {
        circleHitBox.transform.localScale += new Vector3(reapSpeed, 0, reapSpeed);
    }

    public void BeginAttack()
    {
        IsAttacking = true;
        circleHitBox.SetActive(true);
    }

    private void Dash()
    {
        Agent.speed = _fastSpeed;

        if (!IsAttacking)
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
        IsAttacking = false;
        Agent.isStopped = false;
        circleHitBox.SetActive(false);
        circleHitBox.transform.localScale = new Vector3(5, 2, 5);

        anims.SetHit(attackDirection);
        Invoke(nameof(ResetHit), anims.timer);
        */
    }
}
