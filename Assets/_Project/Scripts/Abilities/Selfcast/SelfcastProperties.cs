using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class SelfcastProperties
{
    [Header("Status Effect")]
    public List<StatusEffectProperties> StatusEffectProperties;

    private Stats _casterStats;

    public void ApplyRuntimeData(Stats stats)
    {
        _casterStats = stats;

        foreach (StatusEffectProperties statProps in StatusEffectProperties)
        {
            statProps.ApplyRuntimeData(_casterStats);
        }
    }

    public void ApplyProperties(ProjectileProperties properties)
    {
        StatusEffectProperties = properties.StatusEffectProperties;
    }
}