using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxenHarvesterEvents : MonoBehaviour
{
    public OHActions action;

    public void BeginAttack()
    {
        action.BeginAttack();
    }

    public void StopAttack()
    {
        action.StopAttack();
    }

}
