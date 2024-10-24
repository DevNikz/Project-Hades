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

    [Title("Mana")]
    [Range(0.1f, 1000f)] public float totalMana;
    [ReadOnly][SerializeReference] private float currentMana;

    [Title("Poise")]
    [SerializeField] [Range(0.1f, 100f)] private float totalPoise;
    [SerializeField] [Range(0.1f,1f)] private float poiseMultiplier;
    [ReadOnly] [SerializeReference] private float currentPoise;
    [ReadOnly] [SerializeReference] private bool poiseDamaged;
    [ReadOnly] [SerializeReference] private bool staggered;

    [Title("Hurt Timer (For Animation)")]
    [SerializeField] [Range(0.1f, 5f)] private float timerDelay;
    [ReadOnly] [SerializeReference] private float tempDelay;
    [ReadOnly] [SerializeReference] private TimerState timerState;

    [Title("State")]
    [ReadOnly, SerializeReference] public EntityMovement entityMovement;
    [ReadOnly, SerializeReference] public EntityState entityState;
    [ReadOnly, SerializeReference] public LookDirection lookDirection;
    [ReadOnly, SerializeReference] public Elements elements;
    [ReadOnly, SerializeReference] public Elements selectedElement;
    [ReadOnly, SerializeReference] public bool isDashing;
    [ReadOnly, SerializeReference] public bool isHurt;
    [ReadOnly, SerializeReference] public bool curHurt;
    

    //Ref
    [Title("References")]
    public bool ShowReferences;
    [ShowIfGroup("ShowReferences")]
    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject healthUI;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider healthMeter;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject manaUI;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private Slider manaMeter;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject manaStyleIndicator;

    [BoxGroup("ShowReferences/Reference")]
    [SerializeReference] private GameObject detectUI;

    [BoxGroup("ShowReferences/Reference")]
    [ReadOnly] [SerializeReference] private Vector3 spawnPoint;

    [BoxGroup("ShowReferences/Reference")]
    [ReadOnly] [SerializeReference] private PlayerAnimatorController animatorController;

    public void SetMovement(EntityMovement value) { entityMovement = value; }
    public void SetState(EntityState value) { entityState = value; }
    public void SetDirection(LookDirection value) { lookDirection = value; }
    public void SetElements(Elements value) { elements = value; }
    public void SetSelectedElements(Elements value) { selectedElement = value; }
    public void SetDashing(bool value) { isDashing = value; } 
    public bool IsDashing() { return isDashing; }
    public bool IsHurt() { return isHurt; }

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        healthUI = GameObject.Find("PlayerHealth");
        healthMeter = healthUI.GetComponent<Slider>();

        manaUI = GameObject.Find("PlayerMana");
        manaMeter = manaUI.GetComponent<Slider>();
        manaStyleIndicator = GameObject.Find("StyleIndicator");

        currentPoise = totalPoise;
        currentHealth = totalHealth;
        currentMana = totalMana;
        //spawnPoint = gameObject.transform.position;

        tempDelay = timerDelay;

        animatorController = GetComponent<PlayerAnimatorController>();
    }

    void OnEnable() {
        spawnPoint = gameObject.transform.position;
    }

    void Update(){
        UpdateHealth();
        UpdateAnimatorControllerStates();
    }

    void UpdateAnimatorControllerStates() {
        animatorController.SetMovement(entityMovement);
        animatorController.SetDirection(lookDirection);
        animatorController.SetState(entityState);
        animatorController.SetElements(elements);
        animatorController.SetSelectedElements(selectedElement);
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
            UpdateHurt();
            CheckMovement();
            CheckCombat();
            
            
            // if(sprite.GetComponent<PlayerAnimation>().isActiveAndEnabled == false) {
            //     sprite.GetComponent<PlayerAnimation>().enabled = true;
            // }
            //Debug.Log(PlayerData.entityState);
        }
    }

    void UpdateHurt() {
        if(timerState == TimerState.Start) {
            curHurt = true;
            tempDelay -= Time.deltaTime;
            if(tempDelay <= 0f) timerState = TimerState.Stop;
        }
        if(timerState == TimerState.Stop) {
            curHurt = false;
            isHurt = false;
            timerState = TimerState.None;
        }
    }

    void CheckMovement() {
        if(this.GetComponent<Movement>().isActiveAndEnabled == false) {
            this.GetComponent<Movement>().enabled = true; 
        }
    }

    void CheckCombat() {
        if(this.GetComponent<Combat>().isActiveAndEnabled == false) {
            this.GetComponent<Combat>().enabled = true;
        }
    }

    public void UpdateMana(bool b)
    {
        if (b)
        {
            if (currentMana < totalMana)
                currentMana += 30;
        }
            
        else
        {
            if (currentMana > 0)
                currentMana -= 10;
        }
            
        manaMeter.value = ToPercent(currentMana, totalMana);
    }

    public void UpdateStyleIndicator(string element)
    {
        switch (element)
        {
            case "earth":
                manaStyleIndicator.GetComponent<Image>().color = Color.green;
                break;
            case "fire":
                manaStyleIndicator.GetComponent<Image>().color = Color.red;
                break;
            case "water":
                manaStyleIndicator.GetComponent<Image>().color = Color.cyan;
                break;
            case "wind":
                manaStyleIndicator.GetComponent<Image>().color = Color.yellow;
                break;
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
        if(!staggered && !curHurt) {
            TriggerHurt();
            currentHealth -= damage;
        }
        else if(!staggered && curHurt) {
            isHurt = false;
            Invoke("TriggerHurt", 0.1f);
            currentHealth -= damage;
        }
        
        healthMeter.value = ToPercent(currentHealth, totalHealth);
        CheckHealth();
    }

    void TriggerHurt() {
        TriggerRandomHurtSFX();
        isHurt = true;
        tempDelay = timerDelay;
        timerState = TimerState.Start;
    }

    void TriggerRandomHurtSFX() {
        SFXManager.Instance.Play($"PlayerHurt{Random.Range(1,3)}");
    }

    void CheckHealth() {
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

    public int GetCurrentElementCharge() //int element
    {
        return (int)currentMana;
    }



    //KEEPING FOR IF ELEMENT CHARGES ARE STORED SEPARATELY
    /*return element switch
        {
            1 => currentFireCharge,
            2 => currentEarthCharge,
            3 => currentWaterCharge,
            4 => currentWindCharge,
            _ => -1,
        };*/
    /*[PropertySpace, TitleGroup("Elemental Charges", "Elements Properties", TitleAlignments.Centered)]
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int maxFireCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField] private int currentFireCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int maxWaterCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField] private int currentWaterCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int maxEarthCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField] private int currentEarthCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField][Range(0, 100)] public int maxWindCharge;
    [BoxGroup("Elemental Charges/Box", ShowLabel = false)]
    [SerializeField] private int currentWindCharge;*/
}
