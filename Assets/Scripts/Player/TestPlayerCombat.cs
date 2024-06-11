using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class TestPlayerCombat : MonoBehaviour
{
    public int attackDamage = 20;
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            this.Attack();
        }
    }

    void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider c in hitEnemies)
        {
            c.GetComponent<EnemyCombat>().TakeDamage(this.attackDamage);
            Debug.Log("hit " + c.name);
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(this.attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
