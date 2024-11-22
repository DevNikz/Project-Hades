using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Space] [Title("Properties")]
    [ReadOnly] [SerializeReference] public int deaths; 
    [ReadOnly] [SerializeReference] public GameObject entitySprite;
    [ReadOnly] [SerializeReference] public GameObject pointerSprite;
    [ReadOnly] [SerializeReference] public GameObject deathSprite;
    [ReadOnly] private GameObject deathSpriteTemp;
    [ReadOnly] [SerializeReference] public bool isDead;

    [Space] [Title("Timer")]
    [SerializeField] [Range(0.1f,5f)] public float timer = 2f;
    [ReadOnly] public float tempTimer;
    [ReadOnly] [SerializeReference] public TimerState timerState;



    [PropertySpace] [Title("[Debug]")]
    [Button(ButtonSizes.Gigantic, Name = "Kill Yourself", Icon = SdfIconType.ExclamationCircle), GUIColor("#990c05")] 
    public void KillYourself() {
        this.gameObject.tag = "Player(Dead)";

        //Tick Counter
        deaths += 1;

        //Play Animation / Game over screen

        //Leave a corpse (maybe)
        /* deathSprite = transform.Find("DeathSprite").gameObject;
        Quaternion rot = Quaternion.Euler(90f, Random.Range(0f,360f), 0f);
        deathSpriteTemp = Instantiate(deathSprite, deathSprite.transform.position, rot);
        deathSpriteTemp.tag = "Player(Dead)";
        deathSpriteTemp.SetActive(true); */

        //Respawn Player to same spot
        //entitySprite = transform.Find("SpriteContainer").gameObject;
        //entitySprite.SetActive(false);

        //pointerSprite = transform.Find("Pointer").gameObject;
        //pointerSprite.SetActive(false);
    
        // PlayerController.Instance.entityState = EntityState.Dead;
        // if(PlayerController.Instance.entityState == EntityState.Dead)
        // {
        //     Debug.Log("Player death initiated");

        // }
        // tempTimer = timer;
        // timerState = TimerState.Start;
        
    }


    [PropertySpace] [Button(ButtonSizes.Gigantic, Name = "Forgive Yourself (Reset)")]
    void ResetCounter() {
        deaths = 0;
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Player(Dead)")) {
            Destroy(i);
        }
    }



    void Update() {
        StartTimer();
        isDead = PlayerData.isDead;
        //Debug.Log($"Player state is: {PlayerController.Instance.entityState}");
    }

    void StartTimer() {
        if(timerState == TimerState.Start) {
            tempTimer -= Time.deltaTime;
            if(tempTimer <= 0) {
                timerState = TimerState.Stop;
            }
        }

        /*if(timerState == TimerState.Stop) {
            //Init Vars
            entitySprite.SetActive(true);
            pointerSprite.SetActive(true);

            this.GetComponent<PlayerController>().RevertHealth();

            this.transform.position = this.GetComponent<PlayerController>().GetSpawnPoint();
            this.gameObject.tag = "Player";
            PlayerData.isDead = false;

            //Reset Timer
            timerState = TimerState.None;
        }*/
    }
}
