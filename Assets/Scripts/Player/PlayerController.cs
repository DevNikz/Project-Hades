using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Combat))]
[RequireComponent(typeof(PlayerAnimatorController))]
[RequireComponent(typeof(PlayerDeath))]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

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

    [Title("State")]
    [SerializeReference] public bool isPerformingAction = true;
    public bool returnAction() { return isPerformingAction; }

    [SerializeReference] public EntityState entityState;
    public EntityState currentPlayerState() { return entityState; }

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
    [ReadOnly] [SerializeReference] private Vector3 spawnPoint;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);


        healthUI = GameObject.Find("PlayerHealth");
        healthMeter = healthUI.GetComponent<Slider>();
        currentPoise = totalPoise;
        currentHealth = totalHealth;
        spawnPoint = gameObject.transform.position;
    }

    void OnEnable() {
        spawnPoint = gameObject.transform.position;
    }

    void Update(){
        UpdateHealth();
    }

    void UpdateHealth() {
        if(entityState == EntityState.Dead) {
            this.gameObject.tag = "Player(Dead)";
            if(this.GetComponent<Movement>().isActiveAndEnabled == true) this.GetComponent<Movement>().enabled = false;
            if(this.GetComponent<Combat>().isActiveAndEnabled == true) this.GetComponent<Combat>().enabled = false;
            //if(sprite.GetComponent<PlayerAnimation>().isActiveAndEnabled == true) sprite.GetComponent<PlayerAnimation>().enabled = false;
            //SceneManager.LoadScene("Lose Screen");
        }
        else {
            this.gameObject.tag = "Player";
            if(this.GetComponent<Movement>().isActiveAndEnabled == false) {
                this.GetComponent<Movement>().enabled = true; 
            }
            if(this.GetComponent<Combat>().isActiveAndEnabled == false) {
                this.GetComponent<Combat>().enabled = true;
            }
            // if(sprite.GetComponent<PlayerAnimation>().isActiveAndEnabled == false) {
            //     sprite.GetComponent<PlayerAnimation>().enabled = true;
            // }
            //Debug.Log(PlayerData.entityState);
        }
    }

    public Vector3 GetSpawnPoint() {
        return spawnPoint;
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
        }

        healthMeter.value = ToPercent(currentHealth, totalHealth);

        if(this.currentHealth <= 0) {
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
