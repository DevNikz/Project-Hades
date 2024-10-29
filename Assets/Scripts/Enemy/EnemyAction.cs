using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAction : MonoBehaviour
{
    [SerializeField] public int Action = 0;
    [SerializeField] public GameObject Bullet = null;
    [SerializeField] public int BulletSpeed = 100;
    [SerializeField] public float FireRate = .5f;
    [NonSerialized] public Vector3 originalPosition = Vector3.zero;
    [NonSerialized] public GameObject Player = null;

    [SerializeField] public float moveSpeed = 3;
    [SerializeField] public float rotateSpeed = .6f;
    [SerializeField] public List<Vector3> patrolPoints = new List<Vector3>();

    [NonSerialized] public int nextPoint = 0;
    [NonSerialized]public float timeStep = 0;
    
    [NonSerialized] public Quaternion toRotation = Quaternion.identity;
    [NonSerialized] public Quaternion prevRotation = Quaternion.identity;
    [NonSerialized] public Vector3 direction;

    public bool isAttacking = false;
    public bool isPatrolling = false;
    [NonSerialized] public bool isTurning = false;
    public bool isSearching = false;

    [NonSerialized] public Vector3 lastSeenPos = Vector3.zero;

    [NonSerialized] public Vector3 tempVector;
    [NonSerialized] public float angle;
    [NonSerialized] public Quaternion rot;

    [NonSerialized] public NavMeshAgent agent;

    [SerializeReference] private GameObject sprite;

    private AttackDirection atkDir;

    public float wanderRange = 5;

    public virtual void OnEnable()
    {
        agent = this.GetComponent<NavMeshAgent>();

        this.originalPosition = this.transform.position;
        this.Player = GameObject.Find("Player");

        sprite = transform.Find("SpriteContainer").gameObject;

        this.patrolPoints.Add(this.originalPosition);

        if (this.patrolPoints.Count >= 1) patrolPoints[0] = new Vector3(this.patrolPoints[0].x, this.originalPosition.y, this.patrolPoints[0].z);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.originalPosition.y, this.transform.position.z);

        if (Action != 0) isPatrolling = false;
        if (Action != 1)
        {
            isAttacking = false;
            CancelInvoke();
        }

        agent.isStopped = false;

        switch (Action)
        {
            case 0:
                Patrol();
                break;
            case 1:
                Invoke("Attack", 3.0f);
                break;
            case 2:
                if (!isSearching) this.lastSeenPos = Player.transform.position;
                isSearching = true;
                Search();
                break;
            default:
                this.Action = 0;
                break;
        }
    }

    public void SetAction(int num)
    {
        this.Action = num;
    }

    public void SetPlayerPos(Vector3 pos)
    {
        this.lastSeenPos = pos;
    }

    public virtual void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 randomPoint = this.transform.position + UnityEngine.Random.insideUnitSphere * wanderRange;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        /*
        if (Vector3.Distance(this.transform.position, patrolPoints[nextPoint]) <= 0.1)
        {
            if (this.nextPoint + 1 < patrolPoints.Count)    
                this.nextPoint++;
            else this.nextPoint = 0;

            isPatrolling = false;
        }

        if(!isPatrolling)
        {
            direction = patrolPoints[nextPoint] - this.transform.position;

            prevRotation = this.transform.rotation;

            if (direction != Vector3.zero) //was printing an "error" message in console
                toRotation = Quaternion.LookRotation(direction);

            this.timeStep = 0;
            this.isTurning = true;
            isPatrolling = true;
        }

        if (isTurning)
        {
            agent.isStopped = true;
            this.transform.rotation = Quaternion.Slerp(prevRotation, toRotation, timeStep);
            if (timeStep == 1) isTurning = false;
        }
        else
            agent.destination = patrolPoints[nextPoint];

        this.timeStep += Time.fixedDeltaTime * rotateSpeed;
        if (this.timeStep > 1) this.timeStep = 1;
        */
    }

    public virtual void Attack()
    {
        if (Player != null)
        {
            agent.destination = Player.transform.position;
            if (agent.remainingDistance <= agent.stoppingDistance)
                gameObject.transform.LookAt(Player.transform.position);

            if (!isAttacking) {
                isAttacking = true;
                SetAttackDirection();
                Invoke("Attacking", FireRate);
            }
            
        }
        else {
            Debug.Log("Player not in sight");
            isAttacking = false;
            sprite.GetComponent<EnemyAnimation>().isShooting = false;
        }
    }

    public virtual void Attacking()
    {
        if(isAttacking && this.tag == "Enemy") {
            GameObject fire = GameObject.Instantiate(Bullet);
            if (fire != null)
            {
                fire.transform.position = this.transform.position + (this.transform.forward * 5 / 4);
                fire.transform.rotation = this.transform.rotation;
                fire.transform.Rotate(90, 0, 0);

                fire.SetActive(true);
                if (fire.GetComponent<Rigidbody>() != null)
                    fire.GetComponent<Rigidbody>().AddForce(this.transform.forward * moveSpeed * this.BulletSpeed);

                sprite.GetComponent<EnemyAnimation>().SetShoot(atkDir);
            }
            Invoke("Attacking", FireRate);
        }
    }

    public virtual void Search()
    {
        agent.destination = lastSeenPos;

        if(Vector3.Distance(this.transform.position, lastSeenPos) <= 0.1 || agent.velocity.magnitude == 0)
        {
            this.Action = 0;
            sprite.GetComponent<EnemyAnimation>().isShooting = false;
        }
    }

    void SetAttackDirection()
    {
        if (isAttacking)
        {
            if(Player.transform.position.x < this.transform.position.x) atkDir = AttackDirection.Left;
            else atkDir = AttackDirection.Right;
        }
    }
}
