using Sirenix.OdinInspector;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] private DamageType damageType;
    [SerializeField] private float damage;
    [SerializeField] [Range(5f, 100f)] private float knocbackForce;
    private GameObject tempObject;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    public TimerState timerState;
    [SerializeField] [Range(0f,2f)] private float tempTimer;
    [ReadOnly] [SerializeReference] private Vector3 knockback;
    
    public void StartTimer() {
        timerState = TimerState.Start;
    }

    void Update() {
        UpdateMelee();
    }

    void UpdateMelee() {
        if(timerState == TimerState.Start) {
            tempTimer -= Time.deltaTime;
            if(tempTimer <= 0) {
                timerState = TimerState.Stop;
            }
        }

        if(timerState == TimerState.Stop) {
            this.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        rb = other.gameObject.GetComponent<Rigidbody>();

        if(other.CompareTag("Enemy")) {
            tempObject = other.gameObject;
            other.GetComponent<EnemyController>().ReceiveDamage(damage, damageType);

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            knockback = direction * knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);
        }
    }
}