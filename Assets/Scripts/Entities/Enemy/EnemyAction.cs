
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class EnemyAction : MonoBehaviour
{
    [SerializeField] public int Action = 0;
    [NonSerialized] public float AttackRate;
    [NonSerialized] public GameObject Player = null;
    [NonSerialized] public NavMeshAgent Agent;
    public bool IsAttacking = false;
    public bool IsPatrolling = false;
    public bool IsSearching = false;

    [SerializeField] protected EnemyStatsScriptable _enemyStats;
    [SerializeField] protected GameObject _attackHitbox = null;
    [NonSerialized] private Vector3 _originalPosition = Vector3.zero;
    [NonSerialized] private Vector3 _lastSeenPos = Vector3.zero;
    protected float _wanderRange;
    protected GameObject _sprite;
    protected Rigidbody _rgBody;
    protected AttackDirection _atkDir;
    protected float _maxCooldown;
    public float Cooldown = 0;

    protected EnemyAnimation anims;
    public float timer;

    protected virtual void BonusOnEnable(){}
    /// <summary>
    /// Called every update before calling correct action logic, use to specify behavior of choosing actions
    /// </summary>
    protected abstract void ProcessAILogic();
    protected abstract void Attack();
    protected abstract void Attacking();

    public void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this._originalPosition.y, this.transform.position.z);
        ProcessAILogic();

        if (Cooldown > 0)
        {
            Action = -1;
            Agent.isStopped = true;
            this.IsAttacking = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
            Cooldown -= Time.deltaTime;
            return;
        }
        if (Action == -1) Action = 1;

        if (Action != 0)
        {
            IsPatrolling = false;
            this.gameObject.GetComponentInChildren<SightTrigger>().enabled = false;
        }

        switch (Action)
        {
            case 0:
                Patrol();
                break;
            case 1:
                if (Player != null) Attack();
                else Player = GameObject.Find("Player");
                break;
            case 2:
                if (!IsSearching) this._lastSeenPos = Player.transform.position;
                IsSearching = true;
                Search();
                break;
            default:
                break;
        }
    }

    protected virtual void Search()
    {
        Agent.isStopped = false;
        Agent.destination = _lastSeenPos;
        gameObject.transform.LookAt(_lastSeenPos);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

        if (Vector3.Distance(this.transform.position, _lastSeenPos) <= 0.1 || Agent.velocity.magnitude == 0)
        {
            this.Action = 0;
            anims.isShooting = false;
        }
    }
    
    protected virtual void Patrol()
    {
        this.gameObject.GetComponentInChildren<SightTrigger>().enabled = true;
        Agent.isStopped = false;
        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            Vector3 randomPoint = this.transform.position + UnityEngine.Random.insideUnitSphere * _wanderRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                gameObject.transform.LookAt(hit.position);
                this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
                Agent.SetDestination(hit.position);
            }
        }
    }
    
    /* LIFECYCLE METHODS */
    public virtual void OnEnable()
    {
        _sprite = transform.Find("SpriteContainer").gameObject;
        Agent = this.GetComponent<NavMeshAgent>();
        _rgBody = this.GetComponent<Rigidbody>();
        AttackRate = _enemyStats.attackRate;
        _wanderRange = _enemyStats.wanderRange;
        _maxCooldown = _enemyStats._maxCooldown;

        _originalPosition = this.transform.position;
        Player = GameObject.Find("Player");

        anims = this.GetComponentInChildren<EnemyAnimation>();
        
        BonusOnEnable();
    }

    // ANIMATIONS
    public virtual void SetHit(AttackDirection attackDirection)
    {
        EndAttack();
        Cooldown = 0.5f;

        anims.SetHit(attackDirection);
        Invoke(nameof(ResetHit), anims.timer);
    }

    public virtual void SetStun(AttackDirection attackDirection, float duration)
    {
        EndAttack();
        Cooldown = duration;

        anims.SetStun(attackDirection, duration);
        Invoke(nameof(ResetStun), anims.timer);
    }

    public void ResetHit()
    {
        anims.ResetHit();
    }

    public void ResetStun()
    {
        anims.ResetStun();
    }

    public void EndAttack()
    {
        if (Agent != null)
        {
            Agent.ResetPath();
            Agent.isStopped = true;
        }
        if (this._attackHitbox != null) this._attackHitbox.SetActive(false);

        IsAttacking = false;

        CancelInvoke();
    }

    /* HELPER FUNCTIONS */
    public void SetAction(int num)
    {
        this.Action = num;
    }

    public void SetPlayerPos(Vector3 pos)
    {
        this._lastSeenPos = pos;
    }

    protected void SetAttackDirection()
    {
        if(Player.transform.position.x < this.transform.position.x) _atkDir = AttackDirection.Left;
        else _atkDir = AttackDirection.Right;
    }
}


