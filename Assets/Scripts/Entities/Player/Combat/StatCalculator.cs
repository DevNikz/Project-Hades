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

    public float GetStanceDmgMult(Elements stance, bool hasCharge){
        return (stance, hasCharge) switch {
            (Elements.Earth, true)  => 1.0f,
            (Elements.Earth, false) => 1.0f,
            (Elements.Water, true)  => 1.0f,
            (Elements.Water, false) => 1.0f,
            (Elements.Wind, true)   => 1.0f,
            (Elements.Wind, false)  => 1.0f,
            (Elements.Fire, true)   => 1.0f,
            (Elements.Fire, false)  => 1.0f,
            _                       => 1.0f,
        };
    }

    public float GetStancePoiseDmgMult(Elements stance, bool hasCharge){
        return (stance, hasCharge) switch {
            (Elements.Earth, true)  => 1.0f,
            (Elements.Earth, false) => 1.0f,
            (Elements.Water, true)  => 1.0f,
            (Elements.Water, false) => 1.0f,
            (Elements.Wind, true)   => 1.0f,
            (Elements.Wind, false)  => 1.0f,
            (Elements.Fire, true)   => 1.0f,
            (Elements.Fire, false)  => 1.0f,
            _                       => 1.0f,
        };
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
