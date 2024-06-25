using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameObject tempObject;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    void OnTriggerEnter(Collider other) {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        rb = other.gameObject.GetComponent<Rigidbody>();

        if(other.CompareTag("Bounds") || other.CompareTag("Enemy")) {
            Debug.Log("Bullet hit!");
            tempObject = other.gameObject;
            meshRenderer.material.SetColor("_BaseColor", Color.red);

            Destroy(this.gameObject);
        }
    }

    void OnDestroy() {
        if(tempObject != null) {
            tempObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.white);
        }
    }
}