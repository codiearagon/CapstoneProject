using System;
using UnityEngine;
using System.Collections.Generic;

public class PiercingProjectile : IProjectileHit
{
    private float _damage;
    private Affinity _affinity;
    private List<IStatusEffect> _statusEffects;

    public PiercingProjectile(ProjectileProperties properties, List<IStatusEffect> statusEffects)
    {
        _damage = properties.Damage;
        _affinity = properties.Affinity;
        _statusEffects = statusEffects;
    }

    public void OnHit(Projectile projectile, Collider2D other)
    {
        other?.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);

        foreach(IStatusEffect effect in _statusEffects)
        {
            other?.GetComponent<IStatusEffectable>().Apply(effect);
        }
    }
}
