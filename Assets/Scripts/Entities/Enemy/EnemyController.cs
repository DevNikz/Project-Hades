using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [TitleGroup("Properties", "General Enemy Properties", TitleAlignments.Centered)]
    [SerializeReference] private EnemyStatsScriptable enemyStats;

    [HorizontalGroup("Properties/Group")]
    [VerticalGroup("Properties/Group/Left")]
    [BoxGroup("Properties/Group/Left/Box", ShowLabel = false)]
    [LabelWidth(110)]
    [ReadOnly, SerializeReference] private float currentHealth;

    [BoxGroup("Properties/Group/Left/Box", ShowLabel = false)]
    [LabelWidth(110)]
    [ReadOnly, SerializeReference] private float currentPoise;

    [BoxGroup("Properties/Group/Left/Box", ShowLabel = false)]
    [LabelWidth(110)]
    [ReadOnly, SerializeReference] private float currentTimer;

    [VerticalGroup("Properties/Group/Right")]
    [BoxGroup("Properties/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(110)]
    [ReadOnly, SerializeReference] private bool poiseDamaged;

    [BoxGroup("Properties/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(110)]
    [ReadOnly, SerializeReference] public bool IsStaggered;

    [BoxGroup("Properties/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(110)]
    [ReadOnly, SerializeReference] private bool hasBeenDetained;

    [BoxGroup("Properties/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(110)]
    [ReadOnly, SerializeReference] private TimerState timerState;

    //Ref
    [Title("References")]
    public bool ShowReferences;
    [ShowIfGroup("ShowReferences")]
    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject healthUI;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider healthMeter;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject detectCone;

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

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Vector3 spawnPoint;

    private PlayerController manaCharge;
    private float maxHP;
    void Start() {
        healthUI = this.transform.parent.transform.Find("HealthAndDetection").gameObject;
        detectCone = this.transform.Find("Cone").gameObject;
        // poiseUI = this.transform.parent.transform.Find("Poise").gameObject;
        // poiseMeter = poiseUI.transform.Find("Slider").GetComponent<Slider>();
        hitFX = transform.Find("HitFX").GetComponent<ParticleSystem>();
        sprite = transform.Find("SpriteContainer").gameObject;
        spawnPoint = this.transform.position;
        currentPoise = enemyStats.maxPoise;

        healthMeter = healthUI.transform.Find("HealthSlider").GetComponent<Slider>();

        maxHP = enemyStats.maxHP;
        currentHealth = enemyStats.maxHP;

        if(this.gameObject.TryGetComponent<NavMeshAgent>(out var agent)){
            agent.speed = enemyStats.moveSpeed;
            agent.stoppingDistance = enemyStats.stoppingDistance;
        }
    }

    void SetHealth() {
        healthMeter = healthUI.transform.Find("HealthSlider").GetComponent<Slider>();
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
            this.GetComponent<EnemyAction>().enabled = false;
            detectCone.GetComponent<SightTrigger>().enabled = false;
            this.tag = "Enemy(Dead)";
            healthUI.SetActive(false);
            detectCone.SetActive(false);
        }
        else {
            this.GetComponent<EnemyAction>().enabled = true;
            //detectCone.GetComponent<SightTrigger>().enabled = true;
            healthUI.SetActive(true);
            detectCone.SetActive(true);
        }
    }

    void RegenPoise() {
        if(poiseDamaged && !IsStaggered) {
            currentTimer -= Time.deltaTime;
            if(currentTimer <= 0) {
                timerState = TimerState.Stop;
            }
        }

        if(IsStaggered) {
            currentTimer -= Time.deltaTime;
            if(currentTimer <= 0) {
                IsStaggered = false;
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

    public void RevertPoise() {
        currentPoise = enemyStats.maxPoise;
        //poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);
    }

    public void RevertHealth() {
        currentHealth = enemyStats.maxHP;
        if(enemyStats.enemyType == EnemyType.Normal) healthMeter.value = 1;
        else {
            bossMeter1.value = 1;
            bossMeter2.value = 1;
            bossMeter3.value = 1;
        }
    }

    public Vector3 GetSpawnPoint() {
        return spawnPoint;
    }

    void SFXPlayer(Detain detain) {
        switch(detain) {
            case Detain.Yes:
                SFXManager.Instance.Play("RobotDetained");
                hasBeenDetained = true;
                break;
            case Detain.No:
                SFXManager.Instance.Play("RobotDamaged");
                hasBeenDetained = false;
                break;
        }
    }

    public void ReceiveDamage(DamageType damageType, float damage, float poise, AttackDirection attackDirection, Detain detain) {
        //SFX Play
        SFXPlayer(detain);

        //Visual Cue
        hitFX.Play();
        this.gameObject.GetComponent<EnemyAction>().SetHit(attackDirection);
        manaCharge = FindAnyObjectByType<PlayerController>();

        //Health
        currentHealth -= damage;

        poise = CalculatePoiseDamage(poise);
        currentPoise -= poise;

        if(poise > 0)
            poiseDamaged = true;

        currentTimer = enemyStats.timerDelay;
        //poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);

        if (currentPoise <= 0) 
            this.gameObject.GetComponent<EnemyAction>().SetStun(attackDirection, enemyStats.timerDelay);

        //UI
        switch (enemyStats.enemyType) {
            case EnemyType.Normal: UpdateNormalHP();
                break;
            case EnemyType.Boss: UpdateBossHP();
                break;
        }

        if(this.currentHealth <= 0) {
            this.GetComponent<EnemyDeath>().Die();

            //Add Scrap if ded
            // if(ItemManager.Instance != null) {
            //     ItemManager.Instance.PAddScrap(enemyStats.scrapCount);
            // }
        }
    }

    void UpdateNormalHP() {
        healthMeter.value = ToPercent(currentHealth, maxHP);
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
        return poise * enemyStats.stunResist;
    }

    public bool GetDetain() { return hasBeenDetained; }

    public EnemyStatsScriptable GetStatsScriptable() { return enemyStats; }

    public float getPercentHP() { return currentHealth / maxHP; }
}
