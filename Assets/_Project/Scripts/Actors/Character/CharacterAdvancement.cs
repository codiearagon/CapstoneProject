using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Advancement", menuName = "New Advancement")]
public class CharacterAdvancement : ScriptableObject
{
    public Sprite Icon;
    public Sprite SplashArt;

    [Header("Bonus Stats")]
    public float MaxHp;
    public float MovementSpeed;
    public float Attack;
    public float AttackSpeed;
    public float Defense;

    [Header("Bonus Scalings")]
    public float HpScaling;
    public float AttackScaling;
    public float DefenseScaling;

    [Header("Progression")]
    public int NextAdvancementLevel;
    public List<CharacterAdvancement> Advancements;
}
