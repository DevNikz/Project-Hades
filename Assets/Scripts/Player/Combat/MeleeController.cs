using Sirenix.OdinInspector;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [Title("AttackType")]
    public AttackType attackType;

    [Title("Timer")]
    [SerializeField] [Range(0f,2f)] public float tempTimer;
    [ReadOnly] [SerializeReference] public TimerState timerState;

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
        if(this.gameObject.name == "Melee(Clone)") attackType = Resources.Load<AttackType>("Weapon/Sword/BasicAttack");
        else if(this.gameObject.name == "Lunge(Clone)") attackType = Resources.Load<AttackType>("Weapon/Sword/LungeAttack");
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
            tempObject = other.gameObject;
            other.GetComponent<EnemyController>().ReceiveDamage(attackType.damageType, attackType.damage, attackType.poise, atkdirection);
        }
    }

    public void SetAttackDirection(AttackDirection attackDirection) {
        this.atkdirection = attackDirection;
    }
}