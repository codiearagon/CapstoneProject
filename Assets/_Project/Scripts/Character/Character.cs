using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Action<CharacterStats> OnStatsChanged;

    [field: SerializeField]
    public CharacterBaseSO BaseData { get; private set; }

    [field: SerializeField]
    public CharacterStats Stats { get; private set; }

    private void Awake()
    {
        InitializeStats();   
    }

    private void Start()
    {
        OnStatsChanged.Invoke(Stats);
    }

    private void Update()
    {
        
    }

    private void InitializeStats()
    {
        Stats = new CharacterStats();
        Stats.MaxHp = BaseData.Hp;
        Stats.CurrentHp = BaseData.Hp;
        Stats.MovementSpeed = BaseData.MovementSpeed;
        Stats.Attack = BaseData.Attack;
        Stats.AttackSpeed = BaseData.AttackSpeed;
        Stats.Defense = BaseData.Defense;
    }
}
