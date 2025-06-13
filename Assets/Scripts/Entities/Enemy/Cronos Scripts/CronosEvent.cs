using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CronosEvent : MonoBehaviour
{
    public CronosAI action;

    public void ReapBeginAttack()
    {
        action.BeginAttack();
    }
}
