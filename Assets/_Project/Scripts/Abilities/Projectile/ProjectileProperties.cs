using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ProjectileProperties
{
    public GameObject ProjectilePrefab;

    [Header("Behaviour")]
    public ProjectileMovementBehaviour MovementBehaviour;
    public ProjectileHitBehaviour HitBehaviour;

    [Header("Properties")]
    public bool Rotating;
    public float XSize;
    public float YSize;
    public float Knockback;
    public float Speed;
    public float TimeToLive;

    [Header("Status Effect")]
    public List<StatusEffectProperties> StatusEffectProperties;

    private float _damage;
    private Affinity _affinity;
    private Vector2 _direction;
    private Stats _casterStats;

    public void ApplyRuntimeData(Vector2 direction, Affinity affinity, float damage, Stats stats)
    {
        _direction = direction;
        _affinity = affinity;
        _damage = damage;
        _casterStats = stats;

        foreach(StatusEffectProperties statProps in StatusEffectProperties)
        {
            statProps.ApplyRuntimeData(_casterStats);
        }
    }

    public void ApplyProperties(ProjectileProperties properties)
    {
        ProjectilePrefab = properties.ProjectilePrefab;
        MovementBehaviour = properties.MovementBehaviour;
        HitBehaviour = properties.HitBehaviour;
        Rotating = properties.Rotating;
        XSize = properties.XSize;
        YSize = properties.YSize;
        Knockback = properties.Knockback;
        Speed = properties.Speed;
        TimeToLive = properties.TimeToLive;
        StatusEffectProperties = properties.StatusEffectProperties;
    }

    public float Damage => _damage;
    public Affinity Affinity => _affinity;
    public Vector2 Direction => _direction;
}
