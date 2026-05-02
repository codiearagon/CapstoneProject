using System;
using UnityEngine;

public class GoblinKingBrain : BossBrain
{
    protected override void Phase2()
    {
        base.Phase2();

        if (_phase >= 2)
            return;

        Debug.Log("Phase 2");
        _enemy.MultiplyStat(StatType.MovementSpeed, 1.3f);
        _enemy.MultiplyStat(StatType.AttackSpeed, 1.3f);
    }

    protected override void Phase3()
    {
        base.Phase3();

        if (_phase >= 3)
            return;

        Debug.Log("Phase 3");
        _enemy.MultiplyStat(StatType.MovementSpeed, 1.6f);
        _enemy.MultiplyStat(StatType.AttackSpeed, 1.5f);
        _enemy.MultiplyStat(StatType.Attack, 1.1f);
    }

    protected override void Phase4()
    {
        base.Phase4();

        if (_phase >= 4)
            return;

        Debug.Log("Phase 4");
        _enemy.MultiplyStat(StatType.Defense, 10f);
    }
}