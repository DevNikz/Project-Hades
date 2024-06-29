using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class SightTrigger : MonoBehaviour
{
    [SerializeField] LayerMask layer = 7;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            RaycastHit hit;
            Vector3 parentPos = this.transform.parent.position;
            Vector3 vector = other.gameObject.transform.position - parentPos;
            var distance = vector.magnitude;
            var direction = vector / distance;
            if (Physics.Raycast(parentPos, direction, out hit, distance, layer, QueryTriggerInteraction.Ignore))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
