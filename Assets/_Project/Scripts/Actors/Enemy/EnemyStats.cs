using System;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public string EnemyName;
    public int MaxHp;
    public int CurrentHp;
    public int MovementSpeed;
    public int Attack;
    public float AttackSpeed;
    public int Defense;
    public float AggroRadius;
    public float AttackRange;
}