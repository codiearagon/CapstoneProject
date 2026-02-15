using UnityEngine;

public static class Utility
{
    public static float GetMultiplier(CharacterStats stats, Affinity affinity)
    {
        float multiplier = 0f;

        switch(affinity)
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
}
