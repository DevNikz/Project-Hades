using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LC_Actions : EnemyAction
{

    public Vector3 attackPos = new Vector3(3, 0, -3);
    public float speedMultiplier = 1.5f;
    public float dash = 20;
    public float dashStr = 50;
    public float maxCooldown = 3;
    [NonSerialized] public int comboNum = 0;


    public override void OnEnable()
    {
        agent = this.GetComponent<NavMeshAgent>();
        rgbody = this.GetComponent<Rigidbody>();

        this.originalPosition = this.transform.position;
        this.Player = GameObject.Find("Player");
    }

    public override void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0)
        {
            Action = 0;
            isAttacking = false;
            agent.isStopped = true;
            return;
        }

        if (isAttacking) return;

        if(Action != 1) agent.isStopped = false;

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
    int i = 0;
    public override void Attack()
    {
        if (Player != null)
        {
            if(!agent.isStopped) agent.destination = Player.transform.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                gameObject.transform.LookAt(Player.transform.position);
                this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            }

            if (!isAttacking && Vector3.Distance(this.transform.position, Player.transform.position) < 2)
            {
                comboNum = 3;
                isAttacking = true;
                agent.isStopped = true;
                this.SetAction(comboNum);

                Invoke("Attacking", FireRate);
                Invoke("Attacking", FireRate * 2);
                Invoke("Attacking", FireRate * 3);
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
        this.transform.LookAt(Player.transform.position);
        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

        SetAttackDirection();
        this.Bullet.transform.position = this.transform.position + this.transform.forward;
        this.Bullet.SetActive(true);
        
        comboNum++;

        if (comboNum == 6)
            rgbody.AddForce(this.transform.forward * dashStr, ForceMode.Impulse);
        else
        {
            rgbody.AddForce(this.transform.forward * dash, ForceMode.Impulse);
            this.SetAction(comboNum);
        }

        Invoke("stopAttack", 0.3f);
    }

    public void stopAttack()
    {
        this.Bullet.SetActive(false);

        if (comboNum == 6) {
            this.SetAction(1);
            isAttacking = false;
            cooldown = maxCooldown;
        }
    }
}
