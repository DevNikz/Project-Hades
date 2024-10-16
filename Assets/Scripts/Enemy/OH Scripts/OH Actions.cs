using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class OHActions : EnemyAction
{

    public Vector3 attackPos = new Vector3(3,0,-3);
    public float speedMultiplier = 1.5f;
    private float fastSpeed;
    private float originalSpeed;

    public override void OnEnable()
    {
        agent = this.GetComponent<NavMeshAgent>();

        this.originalPosition = this.transform.position;
        this.Player = GameObject.Find("Player");

        fastSpeed = agent.speed * speedMultiplier;
        originalSpeed = agent.speed;
    }

    public override void Update()
    {
        if (isAttacking) return;

        if (Action != 0) isPatrolling = false;
        if (Action != 1) agent.speed = originalSpeed;

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

    public override void Attack()
    {
        if (Player != null)
        {
            agent.destination = Player.transform.position;
            agent.speed = fastSpeed;

            if (agent.remainingDistance <= agent.stoppingDistance)
                gameObject.transform.LookAt(Player.transform.position);

            if (!isAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < 2)
            {
                isAttacking = true;
                this.SetAction(3);
                this.Bullet.SetActive(true);
                agent.isStopped = true;
                Invoke("Attacking", FireRate);
            }

        }
        else
        {
            Debug.Log("Player not in sight");
            isAttacking = false;
        }
    }

    public override void Attacking()
    {
        if (isAttacking && this.tag == "Enemy")
        {
            this.Bullet.SetActive(false);
            this.SetAction(0);
            isAttacking = false;
            agent.isStopped = false;
        }
    }
}
