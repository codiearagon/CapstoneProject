using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHitProjectile : IProjectileHit
{
    private float _damage;
    private Affinity _affinity;
    private List<IStatusEffect> _statusEffects;

    public DamageOnHitProjectile(ProjectileProperties properties, List<IStatusEffect> statusEffects)
    {
        _damage = properties.Damage;
        _affinity = properties.Affinity;
        _statusEffects = statusEffects;
    }

    public void OnHit(Projectile projectile, Collider2D other)
    {
        other?.GetComponent<ILiving>().TakeDamage(_damage, _affinity);

        foreach (IStatusEffect effect in _statusEffects)
        {
            other?.GetComponent<IStatusEffectable>().ApplyEffect(effect);
        }

        GameObject.Destroy(projectile.gameObject);
    }
}
