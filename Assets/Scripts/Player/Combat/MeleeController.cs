using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] [Range(5f,10f)] private float knocbackForce;
    private GameObject tempObject;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TimerState timerState;
    [SerializeField] [Range(0f,2f)] private float tempTimer;

    void OnEnable() {
        timerState = TimerState.Start;
    }

    void Update() {
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
            meshRenderer.material.SetColor("_BaseColor", Color.red);

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);
        }
    }

    void OnDestroy() {
        if(tempObject != null) {
            tempObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor",Color.white);
        }
    }
}