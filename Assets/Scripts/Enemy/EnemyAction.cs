using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAction : MonoBehaviour
{
    [SerializeField] int Action = 0;
    [SerializeField] GameObject Bullet = null;
    [SerializeField] int BulletSpeed = 500;
    [SerializeField] float FireRate = .5f;
    Vector3 originalPosition = Vector3.zero;
    GameObject Player = null;
    public float StopDis = 2;

    [SerializeField] float moveSpeed = 3;
    [SerializeField] float rotateSpeed = .6f;
    [SerializeField] public List<Vector3> patrolPoints = new List<Vector3>();

    int nextPoint = 0;
    float timeStep = 0;
    
    [NonSerialized] public Quaternion toRotation = Quaternion.identity;
    [NonSerialized] public Quaternion prevRotation = Quaternion.identity;
    [NonSerialized] public Vector3 direction;

    bool isAttacking = false;
    bool isPatrolling = false;
    bool isTurning = false;
    bool isSearching = false;

    [NonSerialized] public Vector3 lastSeenPos = Vector3.zero;

    [NonSerialized] public Vector3 tempVector;
    [NonSerialized] public float angle;
    [NonSerialized] public Quaternion rot;

    [NonSerialized] public NavMeshAgent agent;

    private void OnEnable()
    {
        agent = this.GetComponent<NavMeshAgent>();

        this.originalPosition = this.transform.position;
        this.Player = GameObject.Find("Player");

        this.patrolPoints.Add(this.originalPosition);

        if (this.patrolPoints.Count >= 1) patrolPoints[0] = new Vector3(this.patrolPoints[0].x, this.originalPosition.y, this.patrolPoints[0].z);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.originalPosition.y, this.transform.position.z);

        if (Action != 0) isPatrolling = false;
        if (Action != 1) isAttacking = false;

        agent.isStopped = false;

        switch (Action)
        {
            case 0:
                Patrol();
                break;
            case 1:
                Attack();
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

    void Patrol()
    {
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
    }

    void Attack()
    {
        if (Player != null)
        {

            //Vector3 posPlayer = Player.transform.position;
            //this.transform.LookAt(posPlayer);

            agent.destination = Player.transform.position;
            if (agent.remainingDistance <= agent.stoppingDistance + StopDis)
                agent.isStopped = true;
            //this.transform.position = Vector3.MoveTowards(this.transform.position, posPlayer, moveSpeed * Time.fixedDeltaTime);

            if (!isAttacking) {
                isAttacking = true;
                Invoke("Attacking", FireRate);
            }
            
        }
        else {
            Debug.Log("Player not in sight");
            isAttacking = false;
        }
    }

    void Attacking()
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
            }
            Invoke("Attacking", FireRate);
        }
    }

    void Search()
    {
        agent.destination = lastSeenPos;

        if(Vector3.Distance(this.transform.position, lastSeenPos) <= 0.1)
        {
            this.Action = 0;
        }
    }

    private Vector3 ConvertToIso(float x, float z)
    {

        //North
        if (x == 0 && z == 1) return new Vector3(1f, 0f, 1f);

        //North East
        else if (x == 1 && z == 1) return new Vector3(1f, 0f, 0f);

        //East
        else if (x == 1 && z == 0) return new Vector3(1f, 0f, -1f);

        //South East
        else if (x == 1 && z == -1) return new Vector3(0f, 0f, -1f);

        //South
        else if (x == 0 && z == -1) return new Vector3(-1f, 0f, -1f);

        //South West
        else if (x == -1 && z == -1) return new Vector3(-1f, 0f, 0f);

        //West
        else if (x == -1 && z == 0) return new Vector3(-1f, 0f, 1f);

        //North West
        else if (x == -1 && z == 1) return new Vector3(0f, 0f, 1f);

        else
        {
            Vector3 zero = Vector3.zero;
            return zero;
        }
    }
}
