using System;
using UnityEngine;
using System.Collections.Generic;

public class KnockbackProjectile : IProjectileHit
{
    private float _damage;
    private Affinity _affinity;
    private Vector2 _direction;
    private float _knockback;
    private List<IStatusEffect> _statusEffects;

    public KnockbackProjectile(ProjectileProperties properties, List<IStatusEffect> statusEffects)
    {
        _damage = properties.Damage;
        _affinity = properties.Affinity;
        _direction = properties.Direction;
        _knockback = properties.Knockback;
        _statusEffects = statusEffects;
    }

    public void OnHit(Projectile projectile, Collider2D other)
    {
        other?.GetComponent<IStatEffectable>().ApplyKnockback(_direction * _knockback);
        other?.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);

        foreach (IStatusEffect effect in _statusEffects)
        {
            other?.GetComponent<IStatusEffectable>().ApplyEffect(effect);
        }
    }
}
