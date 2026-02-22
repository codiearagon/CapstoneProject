using System;
using UnityEngine;

public class PiercingProjectile : IProjectileHit
{
    private float _damage;
    private Affinity _affinity;

    public PiercingProjectile(ProjectileProperties _projectileProperties)
    {
        _damage = _projectileProperties.Damage;
        _affinity = _projectileProperties.Affinity;
    }

    public void OnHit(Projectile projectile, Collider2D other)
    {
        other?.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);
    }
}
