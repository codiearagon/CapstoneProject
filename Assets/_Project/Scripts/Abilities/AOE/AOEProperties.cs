using UnityEngine;

[System.Serializable]
public class AOEProperties
{
    public GameObject AoePrefab;

    [Header("Behaviour")]
    public AOEMovementBehaviour MovementBehaviour;
    public AOEHitBehaviour HitBehaviour;

    [Header("Properties")]
    public float Size;
    public float AttackInterval;
    public float TimeToLive;


    private GameObject _caster;
    private Affinity _affinity;
    private float _damage;
    private float _manaCost;
    private IManaUser _manaUser;

    public void ApplyRuntimeData(GameObject caster, Affinity affinity, float damage, float manaCost, IManaUser manaUser)
    {
        _caster = caster;
        _affinity = affinity;
        _damage = damage;
        _manaCost = manaCost;
        _manaUser = manaUser;
    }

    public void ApplyProperties(AOEProperties properties) 
    {
        AoePrefab = properties.AoePrefab;
        Size = properties.Size;
        AttackInterval = properties.AttackInterval;
        TimeToLive = properties.TimeToLive;
    }

    public GameObject Caster => _caster;
    public Affinity Affinity => _affinity;
    public float Damage => _damage;
    public float ManaCost => _manaCost;
    public IManaUser ManaUser => _manaUser;
}
