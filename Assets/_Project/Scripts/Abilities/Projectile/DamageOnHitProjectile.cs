using System;
using UnityEngine;

public class DamageOnHitProjectile : IProjectileHit
{
    private float _damage;
    private Affinity _affinity;

    public DamageOnHitProjectile(float damage, Affinity affinity)
    {
        _damage = damage;
        _affinity = affinity;
    }

    public void OnHit(Projectile projectile, Collider2D other)
    {
        other?.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);
        projectile.Destroy();
    }
}
