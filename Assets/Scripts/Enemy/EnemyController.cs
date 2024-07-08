using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Title("Type")]
    [SerializeField] private EnemyType enemyType;

    [Title("Health")]
    [ReadOnly] [SerializeReference] private float totalHealth;
    [ReadOnly] [SerializeReference] private float currentHealth;

    [Title("Poise")]
    [SerializeField] [Range(0.1f, 100f)] private float totalPoise;
    [SerializeField] [Range(0.1f,1f)] private float poiseMultiplier;
    [ReadOnly] [SerializeReference] private float currentPoise;
    [ReadOnly] [SerializeReference] private bool poiseDamaged;
    [ReadOnly] [SerializeReference] private bool staggered;
    

    [Title("Timer")]
    [SerializeField] [Range(0.1f, 5f)] private float timerDelay;
    [ReadOnly] [SerializeReference] private float tempDelay;
    [ReadOnly] [SerializeReference] private TimerState timerState;

    //Ref
    [Title("References")]
    public bool ShowReferences;
    [ShowIfGroup("ShowReferences")]
    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject healthUI;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider healthMeter;
    
    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider bossMeter1;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider bossMeter2;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider bossMeter3;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject poiseUI;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider poiseMeter;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private ParticleSystem hitFX;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject sprite;

    void Start() {
        healthUI = this.transform.parent.transform.Find("Health").gameObject;
        // poiseUI = this.transform.parent.transform.Find("Poise").gameObject;
        // poiseMeter = poiseUI.transform.Find("Slider").GetComponent<Slider>();
        hitFX = transform.Find("HitFX").GetComponent<ParticleSystem>();
        sprite = transform.Find("SpriteContainer").gameObject;
        currentPoise = totalPoise;

        if(enemyType == EnemyType.Normal) {
            totalHealth = 100;
            SetHealth();
        }
        else {
            totalHealth = 300;
            SetHealthBoss();
        }

        currentHealth = totalHealth;
    }

    void SetHealth() {
        healthMeter = healthUI.transform.Find("Slider").GetComponent<Slider>();
    }

    void SetHealthBoss() {
        bossMeter1 = healthUI.transform.Find("Slider1").GetComponent<Slider>();
        bossMeter2 = healthUI.transform.Find("Slider2").GetComponent<Slider>();
        bossMeter3 = healthUI.transform.Find("Slider3").GetComponent<Slider>();
    }

    void Update() {
        RegenPoise();
        Stagger();
        UpdateHealth();
    }

    void UpdateHealth() {
        if(this.currentHealth <= 0) {
            Destroy(this.GetComponent<EnemyAction>());
            this.gameObject.tag = "Enemy(Dead)";
            this.gameObject.layer = 11;
            healthUI.SetActive(false);
            //poiseUI.SetActive(false);
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
        }
        else {
            this.tag = "Enemy";
        }
    }

    void RevertPoise() {
        currentPoise = totalPoise;
        //poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);
    }

    public void ReceiveDamage(DamageType damageType, float damage, float poise, AttackDirection attackDirection) {
        //Visual Cue
        hitFX.Play();
        sprite.GetComponent<EnemyAnimation>().SetHit(attackDirection);

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
            //poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);
        }

        //UI
        switch(enemyType) {
            case EnemyType.Normal: UpdateNormalHP();
                break;
            case EnemyType.Boss: UpdateBossHP();
                break;
        }

        if(this.currentHealth <= 0) {
            Debug.Log("Dead");
            this.GetComponent<EnemyDeath>().Die();
        }
    }

    void UpdateNormalHP() {
        healthMeter.value = ToPercent(currentHealth, 100);
    }

    void UpdateBossHP() {
        if(currentHealth <= 300 && currentHealth > 200) {
            bossMeter1.value = ToPercent(currentHealth, 300);
        }
        else if(currentHealth <= 200 && currentHealth > 100) {
            bossMeter1.value = ToPercent(currentHealth, 300);
            bossMeter2.value = ToPercent(currentHealth, 200);
        }
        else if(currentHealth <= 100 && currentHealth > 0) {
            bossMeter1.value = 0;
            bossMeter2.value = 0;
            bossMeter3.value = ToPercent(currentHealth, 100);
        }
    }

    float ToPercent(float value, float threshold) {
        return value / threshold;
    }

    float CalculatePoiseDamage(float poise) {
        return poise * poiseMultiplier;
    }
}
