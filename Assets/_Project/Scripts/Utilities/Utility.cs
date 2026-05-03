using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    private static int _pauseRequests = 0;

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

    public static Color GetGoodEventColor() => new Color32(255, 215, 80, 255);
    public static Color GetBadEventColor() => new Color32(220, 50, 50, 255);

    public static float GetMultiplier(Stats stats, Affinity affinity)
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
                return new Color32(255, 80, 40, 255);
            case Affinity.Water:
                return new Color32(0, 110, 255, 255);
            case Affinity.Air:
                return new Color32(0, 255, 200, 255);
            case Affinity.Earth:
                return new Color32(40, 180, 90, 255);
            case Affinity.Dark:
                return new Color32(140, 40, 180, 255);
            case Affinity.Light:
                return new Color32(255, 210, 60, 255);
            default:
                return new Color32(90, 90, 90, 255);
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

    public static float RollRandomPercentage(float min, float max)
    {
        return Random.Range(min, max + 1f) / 100f;
    }

    public static void RequestPause()
    {
        _pauseRequests++;

        if (_pauseRequests >= 1)
            Time.timeScale = 0f;
    }

    public static void ReleasePause()
    {
        _pauseRequests = Mathf.Max(0, _pauseRequests - 1);

        if (_pauseRequests == 0)
            Time.timeScale = 1f;
    }
}
