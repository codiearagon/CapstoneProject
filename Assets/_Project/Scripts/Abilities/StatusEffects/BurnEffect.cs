using System.Collections;
using UnityEngine;

public class BurnEffect : IStatusEffect
{
    private float _duration;
    private float _interval;
    private float _damage;
    private Affinity _affinity;

    public BurnEffect(StatusEffectProperties properties)
    {
        _damage = properties.Damage;
        _interval = properties.Interval;
        _duration = properties.Duration;
        _affinity = properties.Affinity;
    }

    public void Apply(Collider2D target)
    {
        target.GetComponent<IStatusEffectable>().Apply(this);
    }

    public void Remove(Collider2D target)
    {
        target.GetComponent<IStatusEffectable>().Remove(this);
    }

    public IEnumerator Tick(Collider2D target)
    {
        while(_duration > 0)
        {
            yield return new WaitForSeconds(_interval);
            target.GetComponent<IDamageable>().TakeDamage(_damage, _affinity);
            _duration -= _interval;
        }

        Remove(target);
    }
}
