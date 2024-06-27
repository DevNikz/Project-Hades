using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //vars
    [SerializeField] [Range(0.1f,100f)] private float totalHealth;
    [SerializeField] private float currentHealth;

    //Ref
    private Slider meter;
    private ParticleSystem hitFX;
    private ParticleSystem.EmissionModule temp;

    void Start() {
        meter = transform.Find("Health").transform.Find("Slider").GetComponent<Slider>();
        hitFX = transform.Find("HitFX").GetComponent<ParticleSystem>();
        temp = hitFX.emission;
        currentHealth = totalHealth;
    }

    void Update() {

        if(currentHealth == 0) {
            this.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.black);
            this.tag = "Enemy(Dead)";
        }
    }

    public void ReceiveDamage(float damage, DamageType damageType) {
        hitFX.Play();
        currentHealth -= damage;
        meter.value = ToPercent(totalHealth) - ToPercent(currentHealth);
    }

    float ToPercent(float value) {
        return value / 100;
    }
}
