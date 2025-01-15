
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAction : MonoBehaviour
{
    [SerializeField] public int Action = 0;
    [SerializeField] public List<Vector3> PatrolPoints = new();
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
    private float _wanderRange;
    protected GameObject _sprite;
    protected Rigidbody _rgBody;
    protected AttackDirection _atkDir;
    public float Cooldown = 0;

    protected virtual void BonusOnEnable(){}
    /// <summary>
    /// Called every update before calling correct action logic, use to specify behavior of choosing actions
    /// </summary>
    protected abstract void ProcessAILogic();
    protected abstract void Attack();
    protected abstract void Attacking();

    protected virtual void Search()
    {
        Agent.destination = _lastSeenPos;
        gameObject.transform.LookAt(_lastSeenPos);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

        if (Vector3.Distance(this.transform.position, _lastSeenPos) <= 0.1 || Agent.velocity.magnitude == 0)
        {
            this.Action = 0;
            _sprite.GetComponent<EnemyAnimation>().isShooting = false;
        }
    }
    
    protected virtual void Patrol()
    {
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

        _originalPosition = this.transform.position;
        Player = GameObject.Find("Player");

        this.PatrolPoints.Add(this._originalPosition);
        if (this.PatrolPoints.Count >= 1) 
            PatrolPoints[0] = new Vector3(this.PatrolPoints[0].x, this._originalPosition.y, this.PatrolPoints[0].z);
    
        BonusOnEnable();
    }

    public void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this._originalPosition.y, this.transform.position.z);
        ProcessAILogic();

        Cooldown -= Time.deltaTime;
        if (Cooldown > 0)
        {
            Action = 0;
            Agent.isStopped = true;
            IsAttacking = false;
            return;
        }

        if(Action != 0) IsPatrolling = false;
        if (Action != 1)
        {
            IsAttacking = false;
            // CancelInvoke();
        }

        if(Action != 1) Agent.isStopped = false;

        switch (Action)
        {
            case 0:
                Patrol();
                break;
            case 1:
                if(Player != null) Attack();
                else {
                    Player = GameObject.Find("Player");
                    IsAttacking = false;
                }
                break;
            case 2:
                if (!IsSearching) this._lastSeenPos = Player.transform.position;
                IsSearching = true;
                Search();
                break;
            default:
                this.Action = 0;
                break;
        }
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
        if (IsAttacking)
        {
            if(Player.transform.position.x < this.transform.position.x) _atkDir = AttackDirection.Left;
            else _atkDir = AttackDirection.Right;
        }
    }

}
