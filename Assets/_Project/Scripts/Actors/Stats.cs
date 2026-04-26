using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHp,
    HpRegenRate,
    MaxMana,
    ManaRegenRate,
    MovementSpeed,
    Attack,
    AttackSpeed,
    Defense,
    FireMultiplier,
    WaterMultiplier,
    AirMultiplier,
    EarthMultiplier,
    DarkMultiplier,
    LightMultiplier
}

[Serializable]
public class Stats
{
    [Header("Basic Details")]
    public string Name;

    [Header("Properties")]
    public Affinity Affinity;
    public float MaxHp;
    public float CurrentHp;
    public float HpRegenRate;
    public float MaxMana;
    public float CurrentMana;
    public float ManaRegenRate;
    public float MovementSpeed;
    public float Attack;
    public float AttackSpeed;
    public float Defense;
    public float FireMultiplier;
    public float WaterMultiplier;
    public float AirMultiplier;
    public float EarthMultiplier;
    public float DarkMultiplier;
    public float LightMultiplier;

    private List<StatModifier> _modifiers = new List<StatModifier>();

    private ref float GetStat(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHp: return ref MaxHp;
            case StatType.HpRegenRate: return ref HpRegenRate;
            case StatType.MaxMana: return ref MaxMana;
            case StatType.ManaRegenRate: return ref ManaRegenRate;
            case StatType.MovementSpeed: return ref MovementSpeed;
            case StatType.Attack: return ref Attack;
            case StatType.AttackSpeed: return ref AttackSpeed;
            case StatType.Defense: return ref Defense;
            case StatType.FireMultiplier: return ref FireMultiplier;
            case StatType.WaterMultiplier: return ref WaterMultiplier;
            case StatType.AirMultiplier: return ref AirMultiplier;
            case StatType.EarthMultiplier: return ref EarthMultiplier;
            case StatType.DarkMultiplier: return ref DarkMultiplier;
            case StatType.LightMultiplier: return ref LightMultiplier;
            default: return ref MaxHp;
        }
    }

    private float GetBaseValue(StatType type)
    {
        float value = GetStat(type);
        return value;
    }

    public void PermanentBuff(StatType type, float multiplier)
    {
        ref float stat = ref GetStat(type);
        stat += stat * multiplier;
    }

    public void AddModifier(StatModifier modifier)
    {
        _modifiers.Add(modifier);
    }

    public void RemoveModifier(object source)
    {
        _modifiers.RemoveAll(m => m.Source == source);
    }

    public float GetValue(StatType stat)
    {
        float value = GetBaseValue(stat);

        foreach (StatModifier modifier in _modifiers)
        {
            if (modifier.Stat == stat)
                value *= modifier.Multiplier;
        }

        return value;
    }
}

public class StatModifier
{
    public StatType Stat;
    public float Multiplier;
    public object Source;

    public StatModifier(StatType stat, float multiplier, object source)
    {
        Stat = stat;
        Multiplier = multiplier;
        Source = source;
    }
}