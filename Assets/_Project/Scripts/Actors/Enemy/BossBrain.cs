using System;
using UnityEngine;

public abstract class BossBrain : EnemyBrain
{
    public event System.Action<int> OnPhaseChange;

    protected int _phase;

    protected override void Awake()
    {
        base.Awake();

        _phase = 1;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _enemy.OnDamage += HandleDamage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _enemy.OnDamage -= HandleDamage;
    }

    protected virtual void HandleDamage(float currentHealth, float maxHealth)
    {
        CheckPhase(currentHealth / maxHealth);
    }

    protected virtual void CheckPhase(float healthPercent)
    {
        if (healthPercent <= 0.25f)
            Phase4();
        else if (healthPercent <= 0.5f)
            Phase3();
        else if (healthPercent <= 0.75f)
            Phase2();
    }

    protected virtual void Phase2()
    {
        if (_phase >= 2)
            return;

        _phase = 2;
        OnPhaseChange?.Invoke(_phase);
        RecalculateActions();
    }

    protected virtual void Phase3()
    {
        if (_phase >= 3)
            return;

        _phase = 3;
        OnPhaseChange?.Invoke(_phase);
        RecalculateActions();
    }

    protected virtual void Phase4()
    {
        if (_phase >= 4)
            return;

        _phase = 4;
        OnPhaseChange?.Invoke(_phase);
        RecalculateActions();
    }
}