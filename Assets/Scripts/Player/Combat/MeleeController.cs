using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private GameObject tempObject;
    private MeshRenderer meshRenderer;

    void OnTriggerEnter(Collider other) {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        if(other.CompareTag("Enemy")) {
            tempObject = other.gameObject;
            meshRenderer.material.SetColor("_BaseColor", Color.red);
        }
    }

    void OnDestroy() {
        if(tempObject != null) {
            tempObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor",Color.white);
        }
    }
}