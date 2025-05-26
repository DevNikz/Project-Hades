using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackStat", menuName = "ProjectHades/Player/PlayerAttackStat", order = 2)]
[InlineEditor]
public class RevampPlayerAttackStatsScriptable : ScriptableObject
{
    public float BaseDamage;
    public float BasePoiseDamage;
    public float BaseCritRate;
    public float BaseKnockback;
    public float ChargeCost;
    public float ChargeGain;
    public float AutoLungeSpeed;
    public float MaxMoveSpeed;
    public string AnimationClipName;
    public string VFXAnimClipName;
    public float HitboxTiming;
    public float HitboxLingerTime;
    public float EarliestTimeForNextAttack;
    public float ComboInputWindowMaxTime;
    public float AnimationHoldLength;
    public float AttackForgottenTime;
    public float ComboMissPunishTime;
}
