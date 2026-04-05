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
    }

    public float Damage => _damage;
}