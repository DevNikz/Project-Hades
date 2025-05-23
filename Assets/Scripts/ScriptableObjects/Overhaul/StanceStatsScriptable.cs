using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum EStance {
    None, Earth, Water, Air, Fire
}

[CreateAssetMenu(fileName = "StanceStatsScriptable", menuName = "ProjectHades/Player/StanceStatsScriptable", order = 3)]
public class StanceStatsScriptable : ScriptableObject
{
    public Sprite StanceIcon;
    public EStance StanceType;
    public float BaseAttackMult;
    public float FullchargeAttackMult;
    public float BaseStaggerMult;
    public float FullchargeStaggerMult;
    public float AttackSpeedMult;
    public float FullchargeAttackSpeedMult;
    public float AttackRangeMult;
    public float FullchargeAttackRangeMult;
    public List<RevampPlayerAttackStatsScriptable> NormalAttacks = new();
    public List<RevampPlayerAttackStatsScriptable> SpecialAttacks = new();
}