using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Advancement", menuName = "New Advancement")]
public class CharacterAdvancement : ScriptableObject
{
    public string AdvancementName;
    [TextArea] public string Description;
    [TextArea] public string StatSummary;

    public Sprite Icon;
    public Sprite SplashArt;
    public Affinity Affinity;

    [Header("Bonus Stats")]
    public float MaxHp;
    public float HpRegenRate;
    public float MaxMana;
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

    [Header("Special Abilities")]
    public List<Ability> Abilities;

    [Header("Progression")]
    public int NextAdvancementLevel;
    public List<CharacterAdvancement> Advancements;
}
