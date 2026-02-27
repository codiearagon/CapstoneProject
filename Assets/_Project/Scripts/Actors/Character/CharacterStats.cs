using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    [Header("Basic Details")]
    public string CharacterName;

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

    [Header("Experience")]
    public int Level;
    public float ExpToLevelUp;
    public float CurrentExp;

    [Header("Progression")]
    public int NextAbilityUnlockLevel;
    public int NextAbilityUpgradeLevel;
    public int NextAdvancementLevel;

    public ref float GetStat(StatType type)
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
}