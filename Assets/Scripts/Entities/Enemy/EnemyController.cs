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
    [ReadOnly, SerializeReference] private bool staggered;

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
        if(poiseDamaged && !staggered) {
            currentTimer -= Time.deltaTime;
            if(currentTimer <= 0) {
                timerState = TimerState.Stop;
            }
        }

        if(staggered) {
            currentTimer -= Time.deltaTime;
            if(currentTimer <= 0) {
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

        if (staggered) {
            //Health
            currentHealth -= damage * StatCalculator.Instance.StaggeredDmgMult;

            //RegenPoise
            poiseDamaged = false;
            currentTimer = enemyStats.timerDelay;
        }

        //EarthStyle. Basic attacks will be defaulted to EarthStyle - increased stun damage
        else if (MenuScript.LastSelection == 0)
        {
            float earthDamage = damage;
            currentHealth -= earthDamage; //Rudimentary damage increase for now

            //Poise
            poise = CalculatePoiseDamage(poise);

            if(manaCharge.GetCurrentElementCharge() > 0) //Check if player has charge
                currentPoise -= poise * StatCalculator.Instance.EarthPoiseDmgMult(true);
            else currentPoise -= poise * StatCalculator.Instance.EarthPoiseDmgMult(false);

            //For Stun Testing
            //currentPoise -= poise * 100;

            //RegenPoise
            poiseDamaged = true;
            currentTimer = enemyStats.timerDelay;
            //poiseMeter.value = ToPercent(currentPoise, enemyStats.maxPoise);

            Debug.Log("Using earth damage");
        }

        //FireStyle - increased damage
        else if (MenuScript.LastSelection == 1 && manaCharge.GetCurrentElementCharge() > 0)
        {
            float fireDamage;

            if (manaCharge.GetCurrentElementCharge() > 0) //Check if player has charge
                fireDamage = damage * StatCalculator.Instance.FireDmgMult(true);
            else fireDamage = damage * StatCalculator.Instance.FireDmgMult(false);

            currentHealth -= fireDamage;

            //Poise
            poise = CalculatePoiseDamage(poise);
            currentPoise -= poise;

            //RegenPoise
            poiseDamaged = true;
            currentTimer = enemyStats.timerDelay;
            //poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);

            Debug.Log("Using fire damage");
        }

        //WaterStyle - decreased damage, increased aoe (to be done in Combat.cs, see InitHitboxLeft())
        else if (MenuScript.LastSelection == 2)
        {
            float waterDamage = damage * 0.8f;
            currentHealth -= waterDamage; //Rudimentary damage increase for now

            //Poise
            poise = CalculatePoiseDamage(poise);
            currentPoise -= poise;

            //RegenPoise
            poiseDamaged = true;
            currentTimer = enemyStats.timerDelay;
            //poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);

            Debug.Log("Using water damage");
        }

        //WindStyle - decreased damage, higher attack speed ()
        else if (MenuScript.LastSelection == 3)
        {
            float windDamage = damage * 0.8f;
            currentHealth -= windDamage; //Rudimentary damage increase for now

            //Poise
            poise = CalculatePoiseDamage(poise);
            currentPoise -= poise;

            //RegenPoise
            poiseDamaged = true;
            currentTimer = enemyStats.timerDelay;
            //poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);

            Debug.Log("Using wind damage");
        }

        else {
            currentHealth -= damage;

            //Poise
            poise = CalculatePoiseDamage(poise);
            currentPoise -= poise;

            //RegenPoise
            poiseDamaged = true;
            currentTimer = enemyStats.timerDelay;
            //poiseMeter.value = ToPercent(totalPoise) - ToPercent(currentPoise);
        }

        if (currentPoise <= 0) this.gameObject.GetComponent<EnemyAction>().SetStun(attackDirection, enemyStats.timerDelay);

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
