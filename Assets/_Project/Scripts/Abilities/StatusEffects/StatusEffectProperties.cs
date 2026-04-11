using System;
using UnityEngine;

[Serializable]
public class StatusEffectProperties
{
    public StatusEffect Effect;

    [Header("Properties")]
    public Affinity Affinity;
    public float Duration;
    public float Interval;
    public float BaseDamage;
    public float Multiplier;

    private float _damage;

    public void ApplyRuntimeData(Stats _casterStats)
    {
        _damage = BaseDamage * Utility.GetMultiplier(_casterStats, Affinity);
    }

    public void ApplyProperties(StatusEffectProperties properties)
    {
        Effect = properties.Effect;
        Affinity = properties.Affinity;
        Duration = properties.Duration;
        Interval = properties.Interval;
        BaseDamage = properties.BaseDamage;
        Multiplier = properties.Multiplier;
    }

    public float Damage => _damage;
}