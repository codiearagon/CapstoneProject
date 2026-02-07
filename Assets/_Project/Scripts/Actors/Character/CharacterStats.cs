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
    public float MovementSpeed;
    public float Attack;
    public float AttackSpeed;
    public float Defense;

    [Header("Scalings")]
    public float HpScaling;
    public float AttackScaling;
    public float DefenseScaling;

    [Header("Experience")]
    public int Level;
    public float ExpToLevelUp;
    public float CurrentExp;

    [Header("Progression")]
    public int NextAdvancementLevel;
}
