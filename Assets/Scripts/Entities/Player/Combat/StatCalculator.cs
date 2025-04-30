using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatCalculator : MonoBehaviour
{
    [SerializeField] private PlayerElementScriptable earthStats;
    [SerializeField] private PlayerElementScriptable waterStats;
    [SerializeField] private PlayerElementScriptable airStats;
    [SerializeField] private PlayerElementScriptable fireStats;

    public float StaggeredDmgMult {
        get {
            return 2.0f;
        }
    }

    public float EarthDmgMult(bool hasCharge) {
        return 1.0f;
    }

    public float EarthPoiseDmgMult(bool hasCharge) {
        return 1.0f;
    }

    public float WaterDmgMult(bool hasCharge) {
        return 1.0f;
    }

    public float WaterPoiseDmgMult(bool hasCharge) {
        return 1.0f;
    }

    public float AirDmgMult(bool hasCharge) {
        return 1.0f;
    }

    public float AirPoiseDmgMult(bool hasCharge) {
        return 1.0f;
    }

    public float FireDmgMult(bool hasCharge) {
        return 1.0f;
    }

    public float FirePoiseDmgMult(bool hasCharge) {
        return 1.0f;
    }



    public static StatCalculator Instance = null;
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }
}
