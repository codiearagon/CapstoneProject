using System;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    [Header("Basic Details")]
    public string EnemyName;

    [Header("Properties")]
    public Affinity Affinity;
    public float MaxHp;
    public float CurrentHp;
    public float MovementSpeed;
    public float Attack;
    public float AttackSpeed;
    public float Defense;
    public float AttackRange;

    [Header("Progression")]
    public float ExpOnKill;
    public float BuffDropChance;
    public float BuffDropAmount;
}