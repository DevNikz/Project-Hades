using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    public AttackType attackType;
    private GameObject tempObject;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;

    void OnTriggerEnter(Collider other)
    {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        rb = other.gameObject.GetComponent<Rigidbody>();

        if (other.CompareTag("Enemy"))
        {
            tempObject = other.gameObject;

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * attackType.knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);
        }

        if (other.CompareTag("Player"))
        {
            tempObject = other.gameObject;

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * attackType.knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse);

            other.GetComponent<PlayerController>().ReceiveDamage(attackType.damageType, attackType.damage);
        }
    }
}
