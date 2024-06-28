using Sirenix.OdinInspector;
using UnityEngine;

public class Death : MonoBehaviour
{
    [Space] [Title("Properties")]
    [ReadOnly] [SerializeReference] public int deaths; 
    [ReadOnly] [SerializeReference] public GameObject playerSprite;
    [ReadOnly] [SerializeReference] public GameObject deathSprite;
    [ReadOnly] private GameObject deathSpriteTemp;

    [Space] [Title("Timer")]
    [SerializeField] [Range(0.1f,5f)] public float timer = 2f;
    [ReadOnly] public float tempTimer;
    [ReadOnly] [SerializeReference] public TimerState timerState;


    [PropertySpace] [Title("[Debug]")]
    [Button(ButtonSizes.Gigantic, Name = "Kill Yourself", Icon = SdfIconType.ExclamationCircle), GUIColor("#990c05")] 
    void KillYourself() {
        //Tick Counter
        deaths += 1;

        //Play Animation / Game over screen

        //Leave a corpse (maybe)
        deathSprite = transform.Find("DeathSprite").gameObject;
        Quaternion rot = Quaternion.Euler(90f, Random.Range(0f,360f), 0f);
        deathSpriteTemp = Instantiate(deathSprite, deathSprite.transform.position, rot);
        deathSpriteTemp.tag = "Player(Dead)";
        deathSpriteTemp.SetActive(true);

        //Respawn Player to same spot
        playerSprite = transform.Find("SpriteContainer").gameObject;
        playerSprite.SetActive(false);

        tempTimer = timer;
        timerState = TimerState.Start;
    }


    [PropertySpace] [Button(ButtonSizes.Gigantic, Name = "Forgive Yourself (Reset)")]
    void ResetCounter() {
        deaths = 0;
        foreach(GameObject i in GameObject.FindGameObjectsWithTag("Player(Dead)")) {
            Destroy(i);
        }
        //Destroy(GameObject.FindGameObjectsWithTag("Player(Dead)"));
    }

    void Update() {
        StartTimer();
    }

    void StartTimer() {
        if(timerState == TimerState.Start) {
            tempTimer -= Time.deltaTime;
            if(tempTimer <= 0) {
                timerState = TimerState.Stop;
            }
        }

        if(timerState == TimerState.Stop) {
            //Init Vars
            playerSprite.SetActive(true);
            this.transform.position = new Vector3(15.5f, 0.5f, 16f);

            //Reset Timer
            timerState = TimerState.None;
        }
    }
}
