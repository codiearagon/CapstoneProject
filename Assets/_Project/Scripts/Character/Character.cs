using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static event Action<Stats> OnStatsChanged;

    [field: SerializeField]
    public CharacterBaseSO BaseData { get; private set; }

    [field: SerializeField]
    public Stats Stats { get; private set; }

    // Gets ran by PlayerInitialize
    public void Initialize()
    {
        Stats = new Stats();
        Stats.InitializeStats(BaseData);
    }

    private void Start()
    {
        OnStatsChanged?.Invoke(Stats);
    }

    private void Update()
    {
        Logger.Log("Ran Update");
    }

    private void FixedUpdate()
    {
        Logger.Log("Ran FixedUpdate");
    }
}
