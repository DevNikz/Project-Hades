using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        this.currentHealth = this.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        this.currentHealth -= damage;

        this.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);

        Debug.Log("enemy took damage! hp: " + this.currentHealth);

        if(this.currentHealth <= 0)
        {
            this.Die();
        }
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }
}
