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
    [ReadOnly, SerializeReference] public bool IsStaggered = false;
    [ReadOnly, SerializeReference] public bool IsStunned = false;
    [ReadOnly, SerializeReference] public bool IsRusted = false;
    [ReadOnly, SerializeReference] public bool IsSlowed = false;
    [ReadOnly, SerializeReference] public bool IsBurning = false;
    [ReadOnly, SerializeReference] public bool IsAttacking = false;
    [HideInInspector] private float stunTimer;
    [HideInInspector] private float rustTimer;
    [HideInInspector] private float rustBuildup;
    [HideInInspector] private float slowTimer;
    [HideInInspector] private float burnTimer;
    [HideInInspector] private float burnDamageTicker;

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

        stunTimer = 0.0f;
        rustTimer = 0.0f;
        rustBuildup = 0.0f;
        slowTimer = 0.0f;
        burnTimer = 0.0f;
        burnDamageTicker = 0.0f;

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
        UpdateStatusEffects(Time.deltaTime);

        RegenPoise();
        Stagger();
        UpdateHealth();
    }

    private void UpdateStatusEffects(float deltaTime){
        
        if(stunTimer > 0.0f){
            if(!IsStunned)  IsStunned = true;
            stunTimer -= deltaTime;
        } else {
            if(IsStunned){
                IsStunned = false;
                stunTimer = 0.0f;
            }
        }

        if(rustBuildup >= 1.0f){
            rustTimer = ItemManager.Instance.getAugment(AugmentType.Oxidize_Gear).augmentPower2;
            rustBuildup = 1.0f;
        }

        if(rustTimer > 0.0f){
            if(!IsRusted)  IsRusted = true;
            rustTimer -= deltaTime;
        } else {
            if(IsRusted){
                IsRusted = false;
                rustTimer = 0.0f;
                rustBuildup = 0.0f;
            }
        }

        if(slowTimer > 0.0f){
            if(!IsSlowed)  IsSlowed = true;
            slowTimer -= deltaTime;
        } else {
            if(IsSlowed){
                IsSlowed = false;
                slowTimer = 0.0f;
            }
        }

        if(burnTimer > 0.0f){
            if(!IsBurning)  IsBurning = true;
            burnTimer -= deltaTime;
            burnDamageTicker += deltaTime;
        } else {
            if(IsBurning){
                IsBurning = false;
                burnTimer = 0.0f;
                burnDamageTicker = 0.0f;
            }
        }

        if(ItemManager.Instance.getAugment(AugmentType.Immolation_Gear).IsActive){
            while(burnDamageTicker >= ItemManager.Instance.getAugment(AugmentType.Immolation_Gear).augmentPower2){
                burnDamageTicker -= ItemManager.Instance.getAugment(AugmentType.Immolation_Gear).augmentPower2;
                DealBurnDamage(ItemManager.Instance.getAugment(AugmentType.Immolation_Gear).augmentPower);    
            }
            if(burnDamageTicker < 0.0f)
                burnDamageTicker = 0.0f;
        } else {
            while(burnDamageTicker >= ItemManager.Instance.getAugment(AugmentType.Ember_Gear).augmentPower3){
                burnDamageTicker -= ItemManager.Instance.getAugment(AugmentType.Ember_Gear).augmentPower3;
                DealBurnDamage(ItemManager.Instance.getAugment(AugmentType.Ember_Gear).augmentPower2);    
            }
            if(burnDamageTicker < 0.0f)
                burnDamageTicker = 0.0f;
        }
    }

    public void ApplyRust(float amount){
        if(!IsRusted)
            rustBuildup += amount;
    }

    public void Stun(float length){
        if(!IsStunned)
            stunTimer = length;
    }

    public void SetSlow(float length){
        slowTimer = length;
    }

    public void ApplyBurn(float length){
        if(!IsBurning)
            burnTimer = length;
    }

    private void DealBurnDamage(float amount){
        currentHealth -= amount;
        UpdateNormalHP();
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
