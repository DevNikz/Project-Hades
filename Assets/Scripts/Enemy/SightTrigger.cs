using UnityEngine;

public class SightTrigger : MonoBehaviour
{
    [SerializeField] LayerMask layer = 7;
    bool found = false;

    GameObject parentEntity;

    void Start() {
        parentEntity = transform.parent.gameObject;
    }

    void OnEnable() {
        this.GetComponentInParent<EnemyAction>().SetAction(0);
    }

    void OnDisable() {
        parentEntity.GetComponent<EnemyAction>().SetAction(-1);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            RaycastHit hit;
            Vector3 parentPos = this.transform.parent.position;
            Vector3 vector = other.gameObject.transform.position - parentPos;
            var distance = vector.magnitude;
            var direction = vector / distance;
            if (Physics.Raycast(parentPos, direction, out hit, distance, layer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    this.found = true;
                    if (this.GetComponentInParent<EnemyAction>() != null)
                        this.GetComponentInParent<EnemyAction>().SetAction(1);
                }
                else if (this.found && this.GetComponentInParent<EnemyAction>() != null)
                {
                    this.GetComponentInParent<EnemyAction>().SetAction(2);
                    this.GetComponentInParent<EnemyAction>().SetPlayerPos(other.transform.position);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (this.found && this.GetComponentInParent<EnemyAction>() != null)
        {
            this.GetComponentInParent<EnemyAction>().SetAction(2);
            this.GetComponentInParent<EnemyAction>().SetPlayerPos(other.transform.position);
        }
        this.found = false;
    }
}
