using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    private static Dictionary<StatType, float> _statWeights = new Dictionary<StatType, float>()
    {
        [StatType.MaxHp] = 20f,
        [StatType.HpRegenRate] = 12f,
        [StatType.MaxMana] = 20f,
        [StatType.ManaRegenRate] = 10f,
        [StatType.MovementSpeed] = 8f,
        [StatType.Attack] = 30f,
        [StatType.AttackSpeed] = 8f,
        [StatType.Defense] = 12f,
        [StatType.FireMultiplier] = 1f,
        [StatType.WaterMultiplier] = 1f,
        [StatType.AirMultiplier] = 1f,
        [StatType.EarthMultiplier] = 1f,
        [StatType.DarkMultiplier] = 1f,
        [StatType.LightMultiplier] = 1f,
    };

    public static float GetMultiplier(CharacterStats stats, Affinity affinity)
    {
        float multiplier = 0f;

        switch (affinity)
        {
            case Affinity.Fire:
                multiplier = stats.FireMultiplier;
                break;
            case Affinity.Water:
                multiplier = stats.WaterMultiplier;
                break;
            case Affinity.Air:
                multiplier = stats.AirMultiplier;
                break;
            case Affinity.Earth:
                multiplier = stats.EarthMultiplier;
                break;
            case Affinity.Dark:
                multiplier = stats.DarkMultiplier;
                break;
            case Affinity.Light:
                multiplier = stats.LightMultiplier;
                break;
            default:
                multiplier = 1f;
                break;
        }

        return multiplier;
    }

    public static float GetMultiplier(EnemyStats stats, Affinity affinity)
    {
        return 0;
    }

    public static Color GetAffinityColor(Affinity affinity)
    {
        switch (affinity)
        {
            case Affinity.Fire:
                return Color.red;
            case Affinity.Water:
                return Color.blue;
            case Affinity.Air:
                return Color.white;
            case Affinity.Earth:
                return Color.green;
            case Affinity.Dark:
                return Color.violet;
            case Affinity.Light:
                return Color.yellow;
            default:
                return Color.black;
        }
    }

    public static StatType RollRandomStat()
    {
        float totalWeight = 0f;

        foreach(float weight in _statWeights.Values)
        {
            totalWeight += weight;
        }

        float roll = Random.Range(0, totalWeight + 1);

        foreach(KeyValuePair<StatType, float> pair in _statWeights)
        {
            if (roll < pair.Value)
                return pair.Key;

            roll -= pair.Value;
        }

        return _statWeights.Keys.First();
    }

    public static bool RollChance(float probability)
    {
        int rand = Random.Range(1, 101);
        return rand <= probability;
    }
}
