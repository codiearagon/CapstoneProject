using System.Collections;
using UnityEngine;

public class DefShredEffect : IStatusEffect
{
    private float _duration;
    private float _currentDuration;
    private float _interval;
    private float _multiplier;
    private Affinity _affinity;

    public DefShredEffect(StatusEffectProperties properties)
    {
        _multiplier = properties.Multiplier;
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
        target.GetComponent<IStatEffectable>().ApplyStatChange(StatType.Defense, 1f);
        target.GetComponent<IStatusEffectable>().RemoveEffect(this);
    }

    public IEnumerator Tick(Collider2D target)
    {
        target.GetComponent<IStatEffectable>().ApplyStatChange(StatType.Defense, _multiplier);
        while (_currentDuration > 0)
        {
            yield return new WaitForSeconds(_interval);
            _currentDuration -= _interval;
        }

        RemoveEffect(target);
    }
}
