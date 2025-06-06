
using UnityEngine;

public class BugEAI : EnemyAction
{
    [SerializeField] private float _bulletSpeed = 10;
    private ObjectPool bulletPool;
    public int shotCount = 0;

    protected override void BonusOnEnable() {
        bulletPool = GetComponent<ObjectPool>();
    }

    protected override void ProcessAILogic(){
        
    }

    protected override void Attack()
    {
        Agent.isStopped = false;
        Agent.SetDestination(Player.transform.position);

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            gameObject.transform.LookAt(Player.transform.position);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);

            if (Vector3.Distance(this.transform.position, Player.transform.position) <= Agent.stoppingDistance + 1) {
            IsAttacking = true;
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

    public void Shoot()
    {
        SetAttackDirection();
        Attacking();
    }

    protected override void Attacking()
    {
        if(IsAttacking && this.tag == "Enemy") {
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
