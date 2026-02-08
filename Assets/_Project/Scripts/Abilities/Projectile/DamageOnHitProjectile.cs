using System;
using UnityEngine;

[Serializable]
public class DamageOnHitProjectile : IProjectileHit
{
    public void OnHit(Projectile projectile, Collider2D other)
    {
        other?.GetComponent<IDamageable>().TakeDamage(projectile.Damage, projectile.Affinity);
        projectile.Destroy();
    }
}
