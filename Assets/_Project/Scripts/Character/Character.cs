using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static event Action<Stats> OnStatsChanged;

    [field: SerializeField]
    public CharacterBaseSO BaseData { get; private set; }

    [field: SerializeField]
    public Stats Stats { get; private set; }

    private void Awake()
    {
        BaseData = PlayerManager.Instance.Character;
        Stats.InitializeStats(BaseData);
    }

    private void Start()
    {
        OnStatsChanged?.Invoke(Stats);
        Logger.Log("Character Initialized");
    }

    private void Update()
    {
        
    }
}
