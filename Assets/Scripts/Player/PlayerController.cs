using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ColliderModule))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(PlayerDeath))]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{

    [Title("Health")]
    [Range(0.1f,1000f)] public float totalHealth;
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
    [SerializeReference] private GameObject detectUI;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider healthMeter;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject sprite;


    void Awake() {
        healthUI = GameObject.Find("PlayerHealth");
        healthMeter = healthUI.GetComponent<Slider>();
        sprite = transform.Find("SpriteContainer").gameObject;
        currentPoise = totalPoise;
        currentHealth = totalHealth;
    }

    void Update(){
        UpdateHealth();
    }

    void UpdateHealth() {
        if(this.currentHealth <= 0) {
            this.gameObject.tag = "Player(Dead)";
        }
    }

    public void RevertHealth() {
        currentHealth = totalHealth;
        healthMeter.value = ToPercent(currentHealth, totalHealth);
    }

    public void ReceiveDamage(DamageType damageType, float damage) {
        if(staggered) {

        }
        else {
            currentHealth -= damage;

            /*
            poise = CalculatePoiseDamage(poise);
            currentPoise -= poise;

            poiseDamaged = true;
            tempDelay = timerDelay;
            */
        }

        healthMeter.value = ToPercent(currentHealth, totalHealth);

        if(this.currentHealth <= 0) {
            Debug.Log("Player Died.");
            this.GetComponent<PlayerDeath>().KillYourself();
        }

    }

    float ToPercent(float value, float threshold) {
        return value / threshold;
    }

    float CalculatePoiseDamage(float poise) {
        return poise * poiseMultiplier;
    }
}
