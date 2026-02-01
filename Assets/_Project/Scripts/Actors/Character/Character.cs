using System;
using UnityEngine;

[RequireComponent(typeof(ActorMovement))]
public class Character : Actor
{
    public static event Action<Stats> OnStatsChanged;

    public CharacterBaseSO BaseData { get; private set; }

    // Gets ran by PlayerInitialize
    public override void InitializeActor(ActorBaseSO baseData)
    {
        Stats = new Stats();
        InputData = new ActorInputData();
        BaseData = baseData as CharacterBaseSO;
        Stats.InitializeStats(BaseData);
    }

    private void Start()
    {
        OnStatsChanged?.Invoke(Stats);
    }

    public void Look(Vector2 lookDir)
    {

    }

    public override void UpdateInputData(Vector2 moveValue, Vector2 lookValue)
    {
        InputData.MoveValue = moveValue;
        InputData.LookValue = lookValue;
    }

    public override void TakeDamage(int amount)
    {
        Stats.CurrentHp = Mathf.Clamp(Stats.CurrentHp - amount, 0, Stats.MaxHp);
    }
}
