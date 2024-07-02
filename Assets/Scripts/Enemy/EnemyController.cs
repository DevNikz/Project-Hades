using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Title("Health")]
    [SerializeField] [Range(0.1f,100f)] private float totalHealth;
    [ReadOnly] [SerializeField] private float currentHealth;

    [Title("Poise")]
    [SerializeField] [Range(0.1f, 100f)] private float totalPoise;
    [SerializeField] [Range(0.1f,1f)] private float poiseMultiplier;
    [ReadOnly] [SerializeField] private float currentPoise;
    [ReadOnly] [SerializeReference] private bool poiseDamaged;
    [ReadOnly] [SerializeReference] private bool staggered;

    

    [Title("Timer")]
    [SerializeField] [Range(0.1f, 5f)] private float timerDelay;
    [ReadOnly] [SerializeReference] private float tempDelay;
    [ReadOnly] [SerializeReference] private TimerState timerState;

    //Ref
    private GameObject healthUI;
    private Slider healthMeter;

    private GameObject poiseUI;
    private Slider poiseMeter;

    private ParticleSystem hitFX;
    private ParticleSystem.EmissionModule temp;

    void Start() {
        healthUI = this.transform.parent.transform.Find("Health").gameObject;
        healthMeter = healthUI.transform.Find("Slider").GetComponent<Slider>();
        poiseUI = this.transform.parent.transform.Find("Poise").gameObject;
        poiseMeter = poiseUI.transform.Find("Slider").GetComponent<Slider>();
        hitFX = transform.Find("HitFX").GetComponent<ParticleSystem>();
        temp = hitFX.emission;
        currentHealth = totalHealth;
        currentPoise = totalPoise;
    }

    void Update() {
        RegenPoise();
        Stagger();

        if(currentHealth <= 0) {
            this.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.black);
            this.tag = "Enemy(Dead)";
            this.gameObject.layer = 11;
            healthUI.SetActive(false);
            poiseUI.SetActive(false);
        }
    }

    void RegenPoise() {
        if(poiseDamaged && !staggered) {
            tempDelay -= Time.deltaTime;
            if(tempDelay <= 0) {
                timerState = TimerState.Stop;
            }
        }

        if(staggered) {
            tempDelay -= Time.deltaTime;
            if(tempDelay <= 0) {
                staggered = false;
                timerState = TimerState.Stop;
            }
        }

        if(timerState == TimerState.Stop) {
            //Revert
            poiseDamaged = false;
            RevertPoise();
            timerState = TimerState.None;
        }
    }

    void Stagger() {
        if(currentPoise <= 0) {
            this.tag = "Enemy(Staggered)";
            Color color = new Color(1f,0.7127394f,0f,1f);
            this.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
        }
        else {
            this.tag = "Enemy";
            this.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.white);
        }
    }

    void RevertPoise() {
        currentPoise = totalPoise;
        poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);
    }

    public void ReceiveDamage(DamageType damageType, float damage, float poise) {
        //Visual Cue
        hitFX.Play();

        if(staggered) {
            //Health
            currentHealth -= damage * 2; //Multiplier hardcoded for now

            //RegenPoise
            poiseDamaged = false;
            tempDelay = timerDelay;
        }

        else {
            currentHealth -= damage;

            //Poise
            poise = CalculatePoiseDamage(poise);
            currentPoise -= poise;

            //RegenPoise
            poiseDamaged = true;
            tempDelay = timerDelay;
            poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);
        }

        //UI
        healthMeter.value = ToPercent(totalHealth) - ToPercent(currentHealth);
    }

    float ToPercent(float value) {
        return value / 100;
    }

    float CalculatePoiseDamage(float poise) {
        return poise * poiseMultiplier;
    }
}
