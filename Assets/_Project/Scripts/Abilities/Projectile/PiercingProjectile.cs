using System;
using UnityEngine;

public class PiercingProjectile : IProjectileHit
{
    private float _damage;
    private Affinity _affinity;

    public PiercingProjectile(ProjectileProperties properties)
    {
        _damage = properties.Damage;
        _affinity = properties.Affinity;
    }

    public void OnHit(Projectile projectile, Collider2D other)
    {
        other?.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);
    }
}
