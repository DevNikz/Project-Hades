using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] [Range(1f,5f)] private float knocbackForce;
    private GameObject tempObject;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    void OnTriggerEnter(Collider other) {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        rb = other.gameObject.GetComponent<Rigidbody>();

        if(other.CompareTag("Enemy")) {
            Debug.Log("Bullet hit Enemy!");
            tempObject = other.gameObject;
            meshRenderer.material.SetColor("_BaseColor", Color.red);

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);

            Destroy(this.gameObject);
        }

        else if (other.CompareTag("Player")) {
            Debug.Log("Bullet hit Player!");
            tempObject = other.gameObject;
            meshRenderer.material.SetColor("_BaseColor", Color.red);

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);
            Destroy(this.gameObject);
        }

        else if(other.CompareTag("Bounds")) {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy() {
        if(tempObject != null) {
            tempObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.white);
        }
    }
}