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
    public float Speed;
    public float TimeToLive;

    private float _damage;
    private Affinity _affinity;
    private Vector2 _direction;

    public void SetDirection(Vector2 direction)
    { 
        _direction = direction; 
    }

    public void SetAffinity(Affinity affinity)
    {
        _affinity = affinity;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public float Damage => _damage;
    public Affinity Affinity => _affinity;
    public Vector2 Direction => _direction;
}
