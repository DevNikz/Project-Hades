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
    [SerializeField] [Range(0.5f,10f)] public float timer = 2f;
    [ReadOnly] public float tempTimer;
    [ReadOnly] [SerializeReference] public TimerState timerState;

    private bool isDead = false;

    private void OnEnable()
    {
        entitySprite = transform.Find("SpriteContainer").gameObject;
    }

    void SFXPlayer() {
        //will check for enemy type later
        switch(GetComponent<EnemyController>().GetDetain()) {
            case false:
                SFXManager.Instance.Play("RobotKilled");
                break;
        }
    }

    public void Die() {
        if (isDead) return;
        isDead = true;

        SFXPlayer();
        entitySprite.GetComponent<EnemyAnimation>().SetDeath();

        this.gameObject.transform.parent.tag = "Enemy(Dead)";

        tempTimer = timer;
        timerState = TimerState.Start;

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
            entitySprite.SetActive(false);

            //Leave a corpse (maybe)
            deathSprite = transform.Find("DeathSprite").gameObject;
            Quaternion rot = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);
            deathSpriteTemp = Instantiate(deathSprite, deathSprite.transform.position, rot);
            deathSpriteTemp.SetActive(true);
            this.tag = "Enemy(Dead)";

            // Respawn Mechanic
            //this.GetComponent<EnemyController>().RevertHealth();
            //this.GetComponent<EnemyController>().RevertPoise();
            //this.transform.position = this.GetComponent<EnemyController>().GetSpawnPoint();
            //this.gameObject.tag = "Enemy";

            //Reset Timer
            timerState = TimerState.None;
        }
    }
}
