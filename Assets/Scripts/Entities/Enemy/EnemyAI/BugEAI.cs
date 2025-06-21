
using UnityEngine;
using UnityEngine.AI;

public class BugEAI : EnemyAction
{
    [SerializeField] private float _bulletSpeed = 10;
    private ObjectPool bulletPool;
    public int shotCount = 0;
    public float runtime = 0;
    public float origStopDis = 0;

    protected override void BonusOnEnable() {
        bulletPool = GetComponent<ObjectPool>();
        origStopDis = _enemyStats.stoppingDistance;
    }

    protected override void ProcessAILogic(){
        if (runtime >= 0) runtime -= Time.deltaTime;
        
        if (Action != 4) Agent.stoppingDistance = origStopDis;
        else if (Action == 4) RunAway();
    }

    protected override void Attack()
    {
        Agent.isStopped = false;
        Agent.SetDestination(Player.transform.position + Calculate());

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            gameObject.transform.LookAt(Player.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

            if (Vector3.Distance(this.transform.position, Player.transform.position) <= Agent.stoppingDistance + 1) {
            this.SetAction(3);
            }
        }

        // Debug.Log("Attempt to attack");
        
        /*
        if (!isAttacking) {
            isAttacking = true;
            SetAttackDirection();
            Invoke("Attacking", FireRate);
        }
        */
    }

    public void RunAway()
    {
        Agent.stoppingDistance = 0;
        this.transform.LookAt(2 * this.transform.position - Player.transform.position);
        this.findPoint(10f);

        if (runtime <= 0)
        {
            this.SetAction(1);
            Agent.stoppingDistance = origStopDis;
        }
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
        }
    }

    public void Shoot()
    {
        SetAttackDirection();
        Attacking();
    }

    protected override void Attacking()
    {
        if(this.tag == "Enemy") {
            // Debug.Log("Attempted to shoot");
            // GameObject fire = GameObject.Instantiate(Bullet);
            gameObject.transform.LookAt(Player.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

            GameObject fire = bulletPool.ReleaseObject();
            if (fire == null) _sprite.GetComponent<EnemyAnimation>().isShooting = false;
            else
            {
                fire.GetComponent<BulletController>().passPoolRef(this.bulletPool);
                fire.transform.position = this.transform.position + (this.transform.forward * 5 / 4);
                fire.transform.rotation = this.transform.rotation;
                fire.transform.Rotate(90, 0, 0);

                fire.SetActive(true);
                if (fire.GetComponent<Rigidbody>() != null)
                    fire.GetComponent<Rigidbody>().AddForce(this.transform.forward * this._bulletSpeed, ForceMode.VelocityChange);

                //_sprite.GetComponent<EnemyAnimation>().SetShoot(_atkDir);
            }
        }
    }

}
