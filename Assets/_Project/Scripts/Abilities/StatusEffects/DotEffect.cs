using System.Collections;
using UnityEngine;

public class DotEffect : IStatusEffect
{
    private float _duration;
    private float _currentDuration;
    private float _interval;
    private float _damage;
    private Affinity _affinity;

    public DotEffect(StatusEffectProperties properties)
    {
        _damage = properties.Damage;
        _interval = properties.Interval;
        _currentDuration = properties.Duration;
        _duration = properties.Duration;
        _affinity = properties.Affinity;
    }

    public void ApplyEffect(Collider2D target)
    {
        target.GetComponent<IStatusEffectable>().ApplyEffect(this);
    }

    public void Refresh()
    {
        _currentDuration = _duration;
    }

    public void RemoveEffect(Collider2D target)
    {
        target.GetComponent<IStatusEffectable>().RemoveEffect(this);
    }

    public IEnumerator Tick(Collider2D target)
    {
        while(_currentDuration > 0)
        {
            yield return new WaitForSeconds(_interval);
            Logger.Log(_currentDuration);
            target.GetComponent<ILiving>().TakeDamage(_damage, _affinity);
            _currentDuration -= _interval;
        }

        RemoveEffect(target);
    }
}
