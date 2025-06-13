using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackStat", menuName = "ProjectHades/Player/PlayerAttackStat", order = 2)]
[InlineEditor]
public class RevampPlayerAttackStatsScriptable : ScriptableObject
{
    public float BaseDamage;
    public float FullChargeDamageScalar;
    public float BasePoiseDamage;
    public float FullChargePoiseScalar;
    public float BaseCritRate;
    public float FullChargeCritScalar;
    public float BaseKnockback;
    public float FullChargeKnockbackScalar;
    public float FullChargeTime;
    public float ManaCost;
    public float ManaReward;
    public float MaxMoveSpeed;
    public string AnimationClipName;
    public string VFXAnimClipName;
    // public float HitboxTiming;
    // public float HitboxLingerTime;
    // public float EarliestTimeForNextAttack;
    // public float ComboInputWindowMaxTime;
    // public float AnimationHoldLength;
    // public float AttackForgottenTime;
    public float ComboMissPunishTime;
}
