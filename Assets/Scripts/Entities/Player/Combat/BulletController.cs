using Sirenix.OdinInspector;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public AttackType attackType;
    private GameObject tempObject;
    private Rigidbody rb;
    private Rigidbody bulletBody;
    private MeshRenderer meshRenderer;

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

    void OnTriggerEnter(Collider other) {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        rb = other.gameObject.GetComponent<Rigidbody>();

        if(other.CompareTag("Enemy")) {
            tempObject = other.gameObject;

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * attackType.knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);

            this.objectPool.ReturnObject(this.gameObject);
        }

        if (other.CompareTag("Player")) {
            tempObject = other.gameObject;

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * attackType.knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);

            other.GetComponent<PlayerController>().ReceiveDamage(attackType.damageType, attackType.damage);
            this.objectPool.ReturnObject(this.gameObject);
        }

        if(other.CompareTag("Bounds")) {
            this.objectPool.ReturnObject(this.gameObject);
        }
    }
}