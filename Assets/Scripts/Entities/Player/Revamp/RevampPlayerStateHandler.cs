using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevampPlayerStateHandler : MonoBehaviour
{
    public EntityState CurrentState;
    public float CurrentHealth;
    public float CurrentCharge;
    public bool IsDead
    {
        get { return CurrentHealth <= 0; }
    }

    void ResetStatus()
    {
        
    }

    public static RevampPlayerStateHandler Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
