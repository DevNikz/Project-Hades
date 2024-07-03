using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [Space] [Title("Properties")]
    [ReadOnly] [SerializeReference] public GameObject entitySprite;
    [ReadOnly] [SerializeReference] public GameObject deathSprite;
    [ReadOnly] private GameObject deathSpriteTemp;


    public void Die() {
        //Leave a corpse (maybe)
        deathSprite = transform.Find("DeathSprite").gameObject;
        Quaternion rot = Quaternion.Euler(90f, Random.Range(0f,360f), 0f);
        deathSpriteTemp = Instantiate(deathSprite, deathSprite.transform.position, rot);
        deathSpriteTemp.SetActive(true);
        this.tag = "Enemy(Dead)";

        //Respawn Player to same spot
        entitySprite = transform.Find("SpriteContainer").gameObject;
        entitySprite.SetActive(false);
    }
}
