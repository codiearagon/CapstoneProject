using System.Collections;
using UnityEngine;

public class SlowEffect : IStatusEffect
{
    private float _duration;
    private float _currentDuration;
    private float _interval;
    private float _multiplier;

    public SlowEffect(StatusEffectProperties properties)
    {
        _interval = properties.Interval;
        _currentDuration = properties.Duration;
        _duration = properties.Duration;
        _multiplier = properties.Multiplier;
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
        target.GetComponent<IStatEffectable>().RemoveStatModifiers(this);
        target.GetComponent<IStatusEffectable>().RemoveEffect(this);
    }

    public IEnumerator Tick(Collider2D target)
    {
        StatModifier slow = new StatModifier(StatType.MovementSpeed, _multiplier, this);
        target.GetComponent<IStatEffectable>().AddStatModifier(slow);
        while (_currentDuration > 0)
        {
            Debug.Log(_currentDuration);
            yield return new WaitForSeconds(_interval);
            _currentDuration -= _interval;
        }

        RemoveEffect(target);
    }
}
