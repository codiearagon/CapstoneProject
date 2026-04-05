using System;
using UnityEngine;

[Serializable]
public class EnemyStats : Stats
{
    [Header("Extra Properties")]
    public float AttackRange;

    [Header("Progression")]
    public float ExpOnKill;
    public float BuffDropChance;
    public float BuffDropAmount;
}