using System;
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
}
