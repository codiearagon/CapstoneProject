using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;

public class DamageAOE : IAOEHit
{
    private float _damage;
    private Affinity _affinity;

    public DamageAOE(AOEProperties properties)
    {
        _damage = properties.Damage;
        _affinity = properties.Affinity;
    }

    public void OnHit(AOE aoe, Collider2D other)
    {
        other.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);
    }
}