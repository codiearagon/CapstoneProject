using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    [Header("Basic Details")]
    public string CharacterName;

    [Header("Properties")]
    public Affinity Affinity;
    public int MaxHp;
    public int CurrentHp;
    public float MovementSpeed;
    public int Attack;
    public float AttackSpeed;
    public int Defense;
}
