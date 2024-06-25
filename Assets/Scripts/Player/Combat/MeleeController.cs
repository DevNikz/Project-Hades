using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] [Range(5f,10f)] private float knocbackForce;
    private GameObject tempObject;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

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