using UnityEngine;

[System.Serializable]
public class AOEProperties
{
    public GameObject AoePrefab;

    [Header("Properties")]
    public float Size;
    public float AttackInterval;
    public float TimeToLive;

    private float _damage;
    private Affinity _affinity;

    public void ApplyRuntimeData(Affinity affinity, float damage)
    {
        _affinity = affinity;
        _damage = damage;
    }

    public void ApplyProperties(AOEProperties properties) 
    {
        AoePrefab = properties.AoePrefab;
        Size = properties.Size;
        AttackInterval = properties.AttackInterval;
        TimeToLive = properties.TimeToLive;
    }

    public float Damage => _damage;
    public Affinity Affinity => _affinity;
}
