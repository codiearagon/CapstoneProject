using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStats : Stats
{
    [Header("Experience")]
    public int Level;
    public float ExpToLevelUp;
    public float CurrentExp;

    [Header("Progression")]
    public int NextAbilityUnlockLevel;
    public int NextAbilityUpgradeLevel;
    public int NextAdvancementLevel;
}