using Sirenix.OdinInspector;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [SerializeField] private RevampPlayerStateHandler _stateHandler;

    [Title("AttackType")]
    public AttackType attackType;

    [PropertySpace, TitleGroup("Properties")]
    [SerializeField] public float healthDamage;
    [SerializeField] public float poiseDamage;

    [Title("Timer")]
    [SerializeField] [Range(0f,2f)] public float tempTimer;
    [ReadOnly] [SerializeReference] public TimerState timerState;

    [TitleGroup("Hitbox Scale")]
    [HorizontalGroup("Hitbox Scale/Split")]
    [TabGroup("Hitbox Scale/Split/Size", "Default")]
    [SerializeField][Range(0f, 10f)] protected float defaultScaleX = 1.8f;
    [TabGroup("Hitbox Scale/Split/Size", "Default")]
    [SerializeField][Range(0f, 10f)] protected float defaultScaleY = 3f;
    [TabGroup("Hitbox Scale/Split/Size", "Default")]
    [SerializeField][Range(0f, 10f)] protected float defaultScaleZ = 1.2f;

    [TabGroup("Hitbox Scale/Split/Size", "Large")]
    [SerializeField][Range(0f, 10f)] protected float largeScaleX = 2.615041f;
    [TabGroup("Hitbox Scale/Split/Size", "Large")]
    [SerializeField][Range(0f, 10f)] protected float largeScaleY = 5.071505f;
    [TabGroup("Hitbox Scale/Split/Size", "Large")]
    [SerializeField][Range(0f, 10f)] protected float largeScaleZ = 1.2f;

    [Title("References")]
    public bool ShowReferences;

    [ShowIfGroup("ShowReferences")]
    [BoxGroup("ShowReferences/References")]
    [ReadOnly] [SerializeReference] private GameObject tempObject;

    [BoxGroup("ShowReferences/References")]
    [ReadOnly] [SerializeReference] private Rigidbody rb;

    [BoxGroup("ShowReferences/References")]
    [ReadOnly] [SerializeReference] private MeshRenderer meshRenderer;
    [ReadOnly] [SerializeReference] private AttackDirection atkdirection;

    [SerializeField] private PlayerController manaCharge;
    private RevampPlayerAttackStatsScriptable _revampedAttackStats = null;
    private StanceStatsScriptable _stanceStats = null;
    private PlayerCombatStats _playerStats = null;
    private Vector3 _playerPosition;
    private float _chargeTime = 0;

    public void SetAttackStats(RevampPlayerAttackStatsScriptable attack, StanceStatsScriptable stance, PlayerCombatStats playerStats, Vector3 playerPos, float chargeTime)
    {
        _revampedAttackStats = attack;
        _stanceStats = stance;
        _playerStats = playerStats;
        _playerPosition = playerPos;
        _chargeTime = chargeTime;
    }

    void Awake() {
        if(gameObject.CompareTag("PlayerMelee"))
        {
            // attackType = Resources.Load<AttackType>("Weapon/Sword/BasicAttack");
            // gameObject.transform.localScale = new Vector3(defaultScaleX, defaultScaleY, defaultScaleZ);
        }
        else if (gameObject.CompareTag("Detain"))
        {
            attackType = Resources.Load<AttackType>("Weapon/Detain");
            gameObject.transform.localScale = new Vector3(defaultScaleX, defaultScaleY, defaultScaleZ);
        }
        else if (gameObject.CompareTag("PlayerMeleeLarge"))
        {
            attackType = Resources.Load<AttackType>("Weapon/Sword/BasicAttack");
            gameObject.transform.localScale = new Vector3(largeScaleX, largeScaleY, largeScaleZ);
        }
    }
    
    public void StartTimer() {
        timerState = TimerState.Start;
    }

    void Update() {
        UpdateMelee();
    }

    void UpdateMelee() {
        if(timerState == TimerState.Start) {
            tempTimer -= Time.deltaTime;
            if(tempTimer <= 0) {
                timerState = TimerState.Stop;
            }
        }

        if(timerState == TimerState.Stop) {
            this.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
        rb = other.gameObject.GetComponent<Rigidbody>();

        //Switch Condition if detain or not
        switch(gameObject.tag) {
            case "Detain":
                TriggerDetain(other);
                break;
            default: //attack combos
                TriggerAttack(other);
                break;
        }
        
    }

    void TriggerAttack(Collider other) {

        // Debug.Log("Attack hit something");
        if(other.CompareTag("HitHazard")) {
            Debug.Log("Hazard Hit!");
            other.GetComponent<HazardController>().InitHazard();
        }


        if (other.TryGetComponent<BulletController>(out var bullet))
        {
            if (ItemManager.Instance.getAugment(AugmentType.Staunch_Impact_Gear).IsActive)
                bullet.ReturnToPool();
            if (ItemManager.Instance.getAugment(AugmentType.Storm_Wall_Gear).IsActive)
                bullet.Reflect(ItemManager.Instance.getAugment(AugmentType.Storm_Wall_Gear).augmentPower);
        }

        
        if(other.TryGetComponent<EnemyController>(out var enemy)){
            // Debug.Log("Hit an enemy");

            if(attackType == null && _revampedAttackStats == null){
                Debug.LogWarning("[COMBAT-WARN]: Attack type when triggered is null");
                return;
            }

            // AWARD ON HIT CHARGE
            if (_stateHandler != null && !enemy.IsDead)
                _stateHandler.GiveCharge(_revampedAttackStats.ManaReward);

            float healthDmgMult = 1.0f;
            float poiseDmgMult = 1.0f;
            float knockbackMult = 1.0f;
            float criticalHitChance = _revampedAttackStats.BaseCritRate;

            // CALC CHARGE ATTACK SCALAR
            float chargeScalar = 0.0f;
            if (_chargeTime > _revampedAttackStats.FullChargeTime)
                chargeScalar = 1.0f;
            else if(_chargeTime > 0.0f && _revampedAttackStats.FullChargeTime != 0.0f)
                chargeScalar = (_chargeTime / _revampedAttackStats.FullChargeTime);

            healthDmgMult += chargeScalar * _revampedAttackStats.FullChargeDamageScalar;
            poiseDmgMult += chargeScalar * _revampedAttackStats.FullChargePoiseScalar;
            knockbackMult += chargeScalar * _revampedAttackStats.FullChargeKnockbackScalar;
            criticalHitChance += chargeScalar * _revampedAttackStats.FullChargeCritScalar;

            if (_stanceStats != null) criticalHitChance += _stanceStats.ExtraCritRate;
            if (_playerStats != null) criticalHitChance += _playerStats.BaseCritRate;

            healthDmgMult += ItemManager.Instance.getAugmentCount(AugmentType.Aggro) * ItemManager.Instance.getAugment(AugmentType.Aggro).augmentPower;
            poiseDmgMult += ItemManager.Instance.getAugmentCount(AugmentType.Heavy) * ItemManager.Instance.getAugment(AugmentType.Heavy).augmentPower;

            if(ItemManager.Instance.getAugment(AugmentType.Amp_Gear).IsActive)
                healthDmgMult += ItemManager.Instance.getAugment(AugmentType.Amp_Gear).augmentPower;

            if(ItemManager.Instance.getAugment(AugmentType.Tsunami_Gear).IsActive)
                poiseDmgMult += ItemManager.Instance.getAugment(AugmentType.Tsunami_Gear).augmentPower;

            if(ItemManager.Instance.getAugment(AugmentType.Galeforce_Gear).IsActive){
                knockbackMult += ItemManager.Instance.getAugment(AugmentType.Galeforce_Gear).augmentPower;
            } else if (ItemManager.Instance.getAugment(AugmentType.Gust_Strike_Gear).IsActive) {
                knockbackMult += ItemManager.Instance.getAugment(AugmentType.Gust_Strike_Gear).augmentPower;
            }

            if(ItemManager.Instance.getAugment(AugmentType.Volt_Gear).IsActive)
                criticalHitChance += ItemManager.Instance.getAugment(AugmentType.Volt_Gear).augmentPower;

            if(ItemManager.Instance.getAugment(AugmentType.Fume_Gear).IsActive){
                healthDmgMult += ItemManager.Instance.getAugment(AugmentType.Fume_Gear).augmentPower * StatCalculator.Instance.SlowedEnemyCount;
                criticalHitChance += ItemManager.Instance.getAugment(AugmentType.Fume_Gear).augmentPower2 * StatCalculator.Instance.SlowedEnemyCount;
            }

            if(enemy.IsStunned){
                if(ItemManager.Instance.getAugment(AugmentType.Double_Impact_Gear).IsActive)
                    poiseDmgMult += ItemManager.Instance.getAugment(AugmentType.Double_Impact_Gear).augmentPower;
            } else {
                // Chance to stun
                if(enemy.IsAttacking){
                    if(ItemManager.Instance.getAugment(AugmentType.Staunch_Impact_Gear).IsActive)
                        enemy.Stun(ItemManager.Instance.getAugment(AugmentType.Staunch_Impact_Gear).augmentPower);
                } else {
                    if(ItemManager.Instance.getAugment(AugmentType.Impact_Gear).IsActive){
                        if (Random.Range(0, 100) < ItemManager.Instance.getAugment(AugmentType.Impact_Gear).augmentPower * 100)
                        enemy.Stun(ItemManager.Instance.getAugment(AugmentType.Impact_Gear).augmentPower2);
                    }
                }                
            }

            if(enemy.IsRusted){
                if(ItemManager.Instance.getAugment(AugmentType.Corrode_Gear).IsActive)
                    poiseDmgMult += ItemManager.Instance.getAugment(AugmentType.Corrode_Gear).augmentPower;
            
                if(ItemManager.Instance.getAugment(AugmentType.Caustic_Gear).IsActive)
                    criticalHitChance += ItemManager.Instance.getAugment(AugmentType.Caustic_Gear).augmentPower;
            } else {
                if(ItemManager.Instance.getAugment(AugmentType.Oxidize_Gear).IsActive)
                    enemy.ApplyRust(ItemManager.Instance.getAugment(AugmentType.Oxidize_Gear).augmentPower);
            }

            if(ItemManager.Instance.getAugment(AugmentType.Haze_Gear).IsActive){
                enemy.SetSlow(ItemManager.Instance.getAugment(AugmentType.Haze_Gear).augmentPower);
            }

            if(ItemManager.Instance.getAugment(AugmentType.Ember_Gear).IsActive){
                enemy.ApplyBurn(ItemManager.Instance.getAugment(AugmentType.Ember_Gear).augmentPower);
            }
    
            if(enemy.IsBurning){
                if(ItemManager.Instance.getAugment(AugmentType.Scorch_Gear).IsActive)
                    healthDmgMult += ItemManager.Instance.getAugment(AugmentType.Scorch_Gear).augmentPower;
                if(ItemManager.Instance.getAugment(AugmentType.Immolation_Gear).IsActive)
                    healthDmgMult += ItemManager.Instance.getAugment(AugmentType.Immolation_Gear).augmentPower3;
            }

            // Calculate Stagger
            if(enemy.IsStaggered){
                if(ItemManager.Instance.getAugment(AugmentType.Breaching_Gear).IsActive)
                    healthDmgMult += ItemManager.Instance.getAugment(AugmentType.Breaching_Gear).augmentPower;

                if(ItemManager.Instance.getAugment(AugmentType.Punish_Gear).IsActive)
                    criticalHitChance += ItemManager.Instance.getAugment(AugmentType.Punish_Gear).augmentPower;

                healthDmgMult += StatCalculator.Instance.StaggeredDmgMult;

            } else {
                float baseKnockback = _revampedAttackStats.BaseKnockback;
                if(_stanceStats != null) baseKnockback += _stanceStats.ExtraKnockback;
                if (_playerStats != null) baseKnockback += _playerStats.BaseKnockback;
                if(_playerPosition == null) _playerPosition = Vector3.zero;

                Vector3 knockbackDir = other.transform.position - _playerPosition;
                Vector3 targetKnockback =  baseKnockback * knockbackMult * knockbackDir.normalized;
                Vector3 knockbackForce = rb.mass * 2.0f * targetKnockback;

                // Debug.Log($"Knockback: {knockbackDir}, {targetKnockback}, {knockbackForce}");
                rb.AddForce(knockbackForce, ForceMode.Impulse); 

                if(ItemManager.Instance.getAugment(AugmentType.Galeforce_Gear).IsActive || ItemManager.Instance.getAugment(AugmentType.Gust_Strike_Gear).IsActive)
                    enemy.ApplyKnockedbackState(StatCalculator.Instance.KnockbackTime);
            }

            bool doesCritDmg = false;
            // Calculate Critical Hit
            if (
                (
                    ItemManager.Instance.getAugment(AugmentType.Turboshock_Gear).IsActive &&
                    enemy.IsAttacking &&
                    Random.Range(0, 100) < ItemManager.Instance.getAugment(AugmentType.Turboshock_Gear).augmentPower * 100
                ) ||
                    Random.Range(0, 100) < criticalHitChance * 100
                )
            {

                if (ItemManager.Instance.getAugment(AugmentType.Crippling_Gear).IsActive)
                    poiseDmgMult += ItemManager.Instance.getAugment(AugmentType.Crippling_Gear).augmentPower;

                if (enemy.IsRusted)
                {
                    if (ItemManager.Instance.getAugment(AugmentType.Caustic_Gear).IsActive)
                        healthDmgMult += ItemManager.Instance.getAugment(AugmentType.Caustic_Gear).augmentPower;
                }

                healthDmgMult += StatCalculator.Instance.CriticalDmgMult;
                poiseDmgMult += StatCalculator.Instance.CriticalPoiseDmgMult;

                doesCritDmg = true;
                Debug.Log("[COMBAT]: Does a crit!");
            }
            else
            {
                // Debug.Log("[COMBAT]: Does normal damage!");
            }

            if(enemy.IsStaggered)
                poiseDmgMult = 0.0f;

            float healthDamage = _revampedAttackStats.BaseDamage;
            float poiseDamage = _revampedAttackStats.BasePoiseDamage;

            if (_stanceStats != null)
            {
                healthDamage += _stanceStats.BaseExtraDamage;
                poiseDamage += _stanceStats.BaseExtraPoiseDmg;
            }
            else Debug.LogWarning("[WARN]: Stance Stats null");

            if (_playerStats != null)
            {
                healthDamage += _playerStats.BaseDamage;
                poiseDamage += _playerStats.BasePoiseDamage;
            }
            else Debug.LogWarning("[WARN]: Player Stats null");

            healthDamage *= healthDmgMult;
            poiseDamage *= poiseDmgMult;

            healthDamage = Mathf.Round(healthDamage);
            poiseDamage = Mathf.Round(poiseDamage);

            // enemy.ReceiveDamage(attackType.damageType, healthDamage, poiseDamage, atkdirection, Detain.No, doesCritDmg);
            enemy.ReceiveDamage(DamageType.Physical, healthDamage, poiseDamage, atkdirection, Detain.No, doesCritDmg);
        }
    }

    void TriggerDetain(Collider other) {
        if(other.CompareTag("Enemy")) {
            tempObject = other.gameObject;
            other.GetComponent<EnemyController>().ReceiveDamage(attackType.damageType, healthDamage, poiseDamage, atkdirection,  Detain.Yes);

            // No Knockback?
            // Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            // Vector3 knockback = direction * attackType.knocbackForce;
            // rb.AddForce(knockback, ForceMode.Impulse); 
        }

        // Does this apply to staggered enemies?
        // if(other.CompareTag("Enemy(Staggered)")) {
        //     Debug.Log("Enemy Staggered");
        //     tempObject = other.gameObject;

        //     other.GetComponent<EnemyController>().DetainEntity(attackType.damageType, attackType.damage, attackType.poise, atkdirection);
        // }
    } 

    public void SetAttackDirection(AttackDirection attackDirection) {
        this.atkdirection = attackDirection;
    }

    public void SetHealthDamage(float value) { healthDamage = value; }
    public void SetStunDamage(float value) { poiseDamage = value; }
}