using Sirenix.OdinInspector;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public AttackType attackType;
    [SerializeField] public bool IsHostile;
    private GameObject tempObject;
    private Rigidbody rb;
    private Rigidbody bulletBody;
    private MeshRenderer meshRenderer;
    private float _damageScaler = 1.0f;

    [SerializeField][Range(0.1f, 100f)] private float timer;
    private float timerProgress;
    private TimerState timerState = TimerState.None;
    private ObjectPool objectPool;

    public void passPoolRef(ObjectPool pool){
        objectPool = pool;
    }

    void Start() {
        // attackType = Resources.Load<AttackType>(this.attackType.); 
    }

    void OnEnable() {
        this.bulletBody = this.gameObject.GetComponent<Rigidbody>();
        timerState = TimerState.Start;
        timerProgress = timer;
    }

    private void OnDisable() {
        this.bulletBody.velocity = Vector3.zero;    
        this.bulletBody.angularVelocity = Vector3.zero;
    }

    void Update() {
        if(timerState == TimerState.Start) {
            timerProgress -= Time.fixedDeltaTime;
            if(timerProgress <= 0) {
                timerState = TimerState.Stop;
            }
        }
        else if(timerState == TimerState.Stop) {
            this.objectPool.ReturnObject(gameObject);
        }
    }

    public void ReturnToPool(){
        this.objectPool.ReturnObject(this.gameObject);
        _damageScaler = 1.0f;
    }

    public void Reflect(float damageScaler){
        _damageScaler = damageScaler;
        if(this.TryGetComponent<Rigidbody>(out var rb)){
            Vector3 velocity = rb.velocity;
            velocity.x = -velocity.x;
            velocity.y = -velocity.y;
            velocity.z = -velocity.z;
            rb.velocity = velocity;
        }
    }

    void OnTriggerEnter(Collider other) {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        rb = other.gameObject.GetComponent<Rigidbody>();

        if(other.TryGetComponent<EnemyController>(out var enemy)) {
            if(!IsHostile){
                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
                Vector3 knockback = direction * attackType.knocbackForce;
                rb.AddForce(knockback, ForceMode.Impulse);
                enemy.ReceiveDamage(attackType.damageType, attackType.damage * _damageScaler, attackType.poise, AttackDirection.None, Detain.No);
            }

            ReturnToPool();
        }

        if (other.TryGetComponent<PlayerController>(out var player)) {
            if (IsHostile) {
                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
                Vector3 knockback = direction * attackType.knocbackForce;
                rb.AddForce(knockback, ForceMode.Impulse);
                player.ReceiveDamage(attackType.damageType, attackType.damage);
            }
            ReturnToPool();
        }

        if (other.TryGetComponent<RevampPlayerStateHandler>(out var revampPlayer))
        {
            if (IsHostile)
            {
                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
                Vector3 knockback = direction * attackType.knocbackForce;
                rb.AddForce(knockback, ForceMode.Impulse);
                revampPlayer.ReceiveDamage(attackType.damageType, attackType.damage);
            }
            ReturnToPool();
        }

        if(other.CompareTag("Bounds")) {
            ReturnToPool();
        }
    }
}