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
        target.GetComponent<IMoveEffectable>().ApplyMoveSpeed(1f);
        target.GetComponent<IStatusEffectable>().RemoveEffect(this);
    }

    public IEnumerator Tick(Collider2D target)
    {
        while (_currentDuration > 0)
        {
            yield return new WaitForSeconds(_interval);
            target.GetComponent<IMoveEffectable>().ApplyMoveSpeed(_multiplier);
            _currentDuration -= _interval;
        }

        RemoveEffect(target);
    }
}
