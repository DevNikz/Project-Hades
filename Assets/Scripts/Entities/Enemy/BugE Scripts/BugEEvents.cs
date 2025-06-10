using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class BugEEvents : MonoBehaviour
{
    public BugEAI action;

    public void OnEnable()
    {
        action = this.gameObject.GetComponentInParent<BugEAI>();
    }

    public void Shoot()
    {
        action.Shoot();
        action.shotCount++;
    }

    public void End()
    {
        if (action.shotCount >= 3)
        {
            action.Cooldown = action.AttackRate;
            action.shotCount = 0;
        }
    }
}
