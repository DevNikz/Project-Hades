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
                // Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.name == "Player")
                    if(this.GetComponentInParent<EnemyAction>() != null)
                        this.GetComponentInParent<EnemyAction>().SetAction(1);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        this.GetComponentInParent<EnemyAction>().SetAction(0);
    }
}
