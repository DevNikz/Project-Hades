using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // if(other.CompareTag("Player")) {
        //     // Debug.Log("Hit!");
        //     //other.GetComponent<PlayerController>().ReceiveDamage(DamageType.Physical, 10f);
        //     other.GetComponent<RevampPlayerStateHandler>().ReceiveDamage(DamageType.Physical, 10f);
        // }
        
        if(other.TryGetComponent<EnemyController>(out var enemy)) {
            enemy.ReceiveDamage(DamageType.Physical, 10f, 500f, AttackDirection.None, Detain.No, false);
        }
    }
}