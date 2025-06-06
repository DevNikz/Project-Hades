
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class EnemyAction : MonoBehaviour
{
    [SerializeField] public int Action = 0;
    [NonSerialized] public float AttackRate;
    [NonSerialized] public GameObject Player = null;
    [NonSerialized] public NavMeshAgent Agent;
    private float _baseSpeed;
    private float _altBaseSpeed;
    [NonSerialized] public bool IsAttacking = false;
    [NonSerialized] public bool IsPatrolling = false;
    [NonSerialized] public bool IsSearching = false;

    [NonSerialized] public EnemyController _controller;
    [NonSerialized] public EnemyStatsScriptable _enemyStats;
    [SerializeField] protected GameObject _attackHitbox = null;
    [NonSerialized] private Vector3 _originalPosition = Vector3.zero;
    [NonSerialized] private Vector3 _lastSeenPos = Vector3.zero;
    protected float _wanderRange;
    protected GameObject _sprite;
    protected Rigidbody _rgBody;
    protected AttackDirection _atkDir;
    protected float _maxCooldown;
    protected float _timerDelay;
    public float Cooldown = 0;
    LineRenderer line = new LineRenderer();
    public bool drawLine = false;

    protected EnemyAnimation anims;

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
        if(Agent.speed == _baseSpeed && Agent.speed != _altBaseSpeed)
            Agent.speed = _altBaseSpeed;
        
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
        else if (Action == -1)
        {
            Action = 1;
            TeleportPoint();
        }

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

        if(drawLine) DrawLine();
    }

    private void DrawLine()
    {
        line.positionCount = Agent.path.corners.Length;
        line.SetPositions(Agent.path.corners);
    }

    protected virtual void Search()
    {
        Agent.isStopped = false;
        Agent.SetDestination(_lastSeenPos);
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
        Agent.isStopped = false;
        this.gameObject.GetComponentInChildren<SightTrigger>().enabled = true;
        
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
        _baseSpeed = Agent.speed;
        _altBaseSpeed = _baseSpeed;
        _rgBody = this.GetComponent<Rigidbody>();

        _controller = this.GetComponent<EnemyController>();
        _enemyStats = this.GetComponent<EnemyController>().GetStatsScriptable();
        AttackRate = _enemyStats.attackRate;
        _wanderRange = _enemyStats.wanderRange;
        _maxCooldown = _enemyStats._maxCooldown;
        _timerDelay = _enemyStats.timerDelay;

        _originalPosition = this.transform.position;
        Player = GameObject.Find("Player");

        anims = this.GetComponentInChildren<EnemyAnimation>();
        
        BonusOnEnable();

        if (drawLine)
        {
            line = this.AddComponent<LineRenderer>();
            //line.sortingOrder = 1;
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.material.color = Color.red;
        }
    }

    public void TeleportPoint()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(this.transform.position, out hit, 10.0f, NavMesh.AllAreas))
        {
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            this.transform.position = hit.position;
        }
    }

    // ANIMATIONS
    public virtual void SetHit(AttackDirection attackDirection)
    {
        Debug.Log("Dmg");
        if (anims.isDead || anims.isStun || anims.isHit) return;

        EndAttack();
        if(Cooldown < _timerDelay) Cooldown = _timerDelay;

        anims.SetHit(attackDirection);
        Invoke(nameof(ResetHit), _timerDelay);
    }

    public virtual void SetStagger(AttackDirection attackDirection, float duration)
    {
        if (anims.isDead || anims.isStun) return;

        EndAttack();
        Cooldown = duration;

        anims.SetStun(attackDirection, duration);
        Invoke(nameof(ResetStun), duration);
    }
    public void SetSpeed(float speed){
        Agent.speed *= speed;
        _altBaseSpeed = _baseSpeed * speed;
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


