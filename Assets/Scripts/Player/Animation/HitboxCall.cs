using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxCall : MonoBehaviour
{
    public GameObject hitBox;
    public string attackTag;
    public bool isDebug;
    public Quaternion pointerRot;
    public float healthDamage;
    public float poiseDamage;
    public AttackDirection direction;

    public void InitHitBox() {       

        //Instantiate hitbox from selected attack type
        hitBox = Instantiate(hitBox, hitBox.transform.position, pointerRot);

        //Init tag based on attack type (i.e. PlayerMelee, etc)
        hitBox.tag = attackTag;

        //Init Stats
        hitBox.GetComponent<MeleeController>().SetHealthDamage(healthDamage);
        hitBox.GetComponent<MeleeController>().SetStunDamage(poiseDamage);

        //Start Timer for hitbox (To mimic ticks)
        hitBox.GetComponent<MeleeController>().StartTimer();
        
        //Set Pos of hitbox
        if(direction == AttackDirection.Right) {
            hitBox.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Right);
        }

        else {
            hitBox.GetComponent<MeleeController>().SetAttackDirection(AttackDirection.Left);
        }

        //MeshRenderer | True = Debug | False = Release
        hitBox.GetComponent<MeshRenderer>().enabled = isDebug;

        //Activate it 
        hitBox.SetActive(true);
    }
}
