using System;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    [Header("Basic Details")]
    public string EnemyName;

    [Header("Properties")]
    public Affinity Affinity;
    public int MaxHp;
    public int CurrentHp;
    public float MovementSpeed;
    public int Attack;
    public float AttackSpeed;
    public int Defense;
    public float AttackRange;
}