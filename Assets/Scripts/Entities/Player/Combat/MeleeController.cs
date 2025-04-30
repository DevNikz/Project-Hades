using Sirenix.OdinInspector;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
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

    void Awake() {
        if(gameObject.CompareTag("PlayerMelee"))
        {
            attackType = Resources.Load<AttackType>("Weapon/Sword/BasicAttack");
            gameObject.transform.localScale = new Vector3(defaultScaleX, defaultScaleY, defaultScaleZ);
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
        if(other.CompareTag("HitHazard")) {
            Debug.Log("Hazard Hit!");
            other.GetComponent<HazardController>().InitHazard();
        }
        
        if(other.TryGetComponent<EnemyController>(out var enemy)){
            Debug.Log("Hit an enemy");

            if(attackType == null){
                Debug.LogWarning("[COMBAT-WARN]: Attack type when triggered is null");
                return;
            }

            float healthDamage = attackType.damage;
            float poiseDamage = attackType.poise;

            switch(MenuScript.LastSelection){
                // EARTH STANCE
                case 0:
                    healthDamage *= StatCalculator.Instance.GetStanceDmgMult(Elements.Earth, manaCharge.GetCurrentElementCharge() > 0);
                    poiseDamage *= StatCalculator.Instance.GetStancePoiseDmgMult(Elements.Earth, manaCharge.GetCurrentElementCharge() > 0);
                    break;

                // FIRE STANCE
                case 1:
                    healthDamage *= StatCalculator.Instance.GetStanceDmgMult(Elements.Fire, manaCharge.GetCurrentElementCharge() > 0);
                    poiseDamage *= StatCalculator.Instance.GetStancePoiseDmgMult(Elements.Fire, manaCharge.GetCurrentElementCharge() > 0);
                    break;

                // WATER STANCE
                case 2:
                    healthDamage *= StatCalculator.Instance.GetStanceDmgMult(Elements.Water, manaCharge.GetCurrentElementCharge() > 0);
                    poiseDamage *= StatCalculator.Instance.GetStancePoiseDmgMult(Elements.Water, manaCharge.GetCurrentElementCharge() > 0);
                    break;

                // WIND STANCE
                case 3:
                    healthDamage *= StatCalculator.Instance.GetStanceDmgMult(Elements.Wind, manaCharge.GetCurrentElementCharge() > 0);
                    poiseDamage *= StatCalculator.Instance.GetStancePoiseDmgMult(Elements.Wind, manaCharge.GetCurrentElementCharge() > 0);
                    break;
            }

            if(enemy.IsStaggered){
                healthDamage *= StatCalculator.Instance.StaggeredDmgMult;
                poiseDamage = 0.0f;

            } else {
                Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
                Vector3 knockback = direction * attackType.knocbackForce;
                rb.AddForce(knockback, ForceMode.Impulse); 
            }

            enemy.ReceiveDamage(attackType.damageType, healthDamage, poiseDamage, atkdirection, Detain.No);
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