using Sirenix.OdinInspector;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [Title("AttackType")]
    public AttackType attackType;

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

        if(other.CompareTag("Enemy")) {
            tempObject = other.gameObject;
            other.GetComponent<EnemyController>().ReceiveDamage(attackType.damageType, attackType.damage, attackType.poise, atkdirection);

            Vector3 direction = (other.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = direction * attackType.knocbackForce;
            rb.AddForce(knockback, ForceMode.Impulse); 
        }

        if(other.CompareTag("Enemy(Staggered)")) {
            Debug.Log("Enemy Staggered");
            tempObject = other.gameObject;
            other.GetComponent<EnemyController>().ReceiveDamage(attackType.damageType, attackType.damage, attackType.poise, atkdirection);
        }
    }

    public void SetAttackDirection(AttackDirection attackDirection) {
        this.atkdirection = attackDirection;
    }
}