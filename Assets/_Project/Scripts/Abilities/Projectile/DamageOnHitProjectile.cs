using System;
using UnityEngine;

public class DamageOnHitProjectile : IProjectileHit
{
    private float _damage;
    private Affinity _affinity;

    public DamageOnHitProjectile(ProjectileProperties properties)
    {
        _damage = properties.Damage;
        _affinity = properties.Affinity;
    }

    public void OnHit(Projectile projectile, Collider2D other)
    {
        other?.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);
        GameObject.Destroy(projectile.gameObject);
    }
}
