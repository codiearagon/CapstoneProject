using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;

public class DamageAOE : IAOEHit
{
    private float _damage;
    private Affinity _affinity;
    private List<IStatusEffect> _statusEffects;

    public DamageAOE(AOEProperties properties, List<IStatusEffect> statusEffects)
    {
        _damage = properties.Damage;
        _affinity = properties.Affinity;
        _statusEffects = statusEffects;
    }

    public void OnHit(AOE aoe, Collider2D other)
    {
        other.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);

        foreach (IStatusEffect effect in _statusEffects)
        {
            other?.GetComponent<IStatusEffectable>().ApplyEffect(effect);
        }
    }
}