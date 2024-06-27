using UnityEngine;

public class LungeController : MonoBehaviour
{
    [SerializeField] [Range(5f,10f)] private float knocbackForce;
    private GameObject tempObject;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TimerState timerState;
    private float templungetimer = 0.25f;

    void OnEnable() {
        timerState = TimerState.Start;
    }

    void Update() {
        UpdateLunge();
    }

    void UpdateLunge() {
        if(timerState == TimerState.Start) {
            templungetimer -= Time.deltaTime;
            if(templungetimer <= 0) {
                timerState = TimerState.Stop;
            }
        }
        if(timerState == TimerState.Stop) {
            templungetimer = 0.25f;
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other) {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        rb = other.gameObject.GetComponent<Rigidbody>();

        if(other.CompareTag("Enemy")) {
            tempObject = other.gameObject;
            other.GetComponent<EnemyController>().ReceiveDamage(25f, DamageType.Physical);

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);
        }
    }
}