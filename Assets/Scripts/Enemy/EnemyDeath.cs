using System.Threading;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [Space] [Title("Properties")]
    [ReadOnly] [SerializeReference] public GameObject entitySprite;
    [ReadOnly] [SerializeReference] public GameObject deathSprite;
    [ReadOnly] private GameObject deathSpriteTemp;

    [Space] [Title("Timer")]
    [SerializeField] [Range(1f,10f)] public float timer = 5f;
    [ReadOnly] public float tempTimer;
    [ReadOnly] [SerializeReference] public TimerState timerState;


    public void Die() {
        //Leave a corpse (maybe)
        deathSprite = transform.Find("DeathSprite").gameObject;
        Quaternion rot = Quaternion.Euler(90f, Random.Range(0f,360f), 0f);
        deathSpriteTemp = Instantiate(deathSprite, deathSprite.transform.position, rot);
        deathSpriteTemp.SetActive(true);
        this.tag = "Enemy(Dead)";

        entitySprite = transform.Find("SpriteContainer").gameObject;
        entitySprite.SetActive(false);

        // tempTimer = timer;
        // timerState = TimerState.Start;

        Broadcaster.Instance.AddBoolParam(Combat.ENEMY_KILLED, EventNames.Combat.ENEMY_KILLED, true);

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
            entitySprite.SetActive(true);
            this.GetComponent<EnemyController>().RevertHealth();
            this.GetComponent<EnemyController>().RevertPoise();
            this.transform.position = this.GetComponent<EnemyController>().GetSpawnPoint();
            this.gameObject.tag = "Enemy";

            //Reset Timer
            timerState = TimerState.None;
        }
    }
}
