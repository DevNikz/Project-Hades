using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightClockworkEvent : MonoBehaviour
{
    public LC_Actions action;

    public void OnEnable()
    {
        action = this.gameObject.GetComponentInParent<LC_Actions>();
    }

    public void TurnOn()
    {
        action.TurnHitOn();
    }

    public void TurnOff()
    {
        action.TurnHitOff();
    }

    public void nextCombo()
    {
        TurnOff();
        action.BeginAttack();
    }

    public void Stop()
    {
        TurnOff();
        action.StopAttack();
    }
}
