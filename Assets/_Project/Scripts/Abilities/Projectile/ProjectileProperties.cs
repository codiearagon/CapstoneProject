using System;
using UnityEngine;

[Serializable]
public class ProjectileProperties
{
    public GameObject ProjectilePrefab;

    [Header("Behaviour")]
    public ProjectileMovementBehaviour MovementBehaviour;
    public ProjectileHitBehaviour HitBehaviour;

    [Header("Properties")]
    public float Size;
    public float Speed;
    public float TimeToLive;

    private float _damage;
    private Affinity _affinity;
    private Vector2 _direction;

    public void ApplyRuntimeData(Vector2 direction, Affinity affinity, float damage)
    {
        _direction = direction;
        _affinity = affinity;
        _damage = damage;
    }

    public void ApplyProperties(ProjectileProperties properties)
    {
        ProjectilePrefab = properties.ProjectilePrefab;
        MovementBehaviour = properties.MovementBehaviour;
        HitBehaviour = properties.HitBehaviour;
        Size = properties.Size;
        Speed = properties.Speed;
        TimeToLive = properties.TimeToLive;
    }

    public float Damage => _damage;
    public Affinity Affinity => _affinity;
    public Vector2 Direction => _direction;
}
