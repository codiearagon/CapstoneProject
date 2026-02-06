using System.Collections.Generic;

public enum Affinity
{
    None,
    Fire,
    Water,
    Air,
    Earth,
    Dark,
    Light
}

public static class AffinityLookup
{
    private static Dictionary<Affinity, Dictionary<Affinity, float>> _table;
 
    static AffinityLookup()
    {
        _table = new Dictionary<Affinity, Dictionary<Affinity, float>>
        {
            [Affinity.Fire] = new Dictionary<Affinity, float>
            {
                [Affinity.Water] = 0.5f,
                [Affinity.Earth] = 2f
            },

            [Affinity.Water] = new Dictionary<Affinity, float>
            {
                [Affinity.Fire] = 2f,
                [Affinity.Air] = 0.5f
            },

            [Affinity.Air] = new Dictionary<Affinity, float>
            {
                [Affinity.Water] = 2f,
                [Affinity.Earth] = 0.5f
            },

            [Affinity.Earth] = new Dictionary<Affinity, float>
            {
                [Affinity.Air] = 2f,
                [Affinity.Fire] = 0.5f
            },

            [Affinity.Dark] = new Dictionary<Affinity, float>
            {
                [Affinity.Light] = 2f,
            },

            [Affinity.Light] = new Dictionary<Affinity, float>
            {
                [Affinity.Dark] = 2f,
            }

        };
    }

    public static float GetMultiplier(Affinity source, Affinity target)
    {
        // No affinity always does 70% of the damage regardless of target type
        if (source == Affinity.None)
            return 0.7f;

        if (_table[source].TryGetValue(target, out float value))
            return value;

        return 1f;
    }
}
