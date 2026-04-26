using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    protected override IEnumerator AttackLoop(Stats stats, GameObject target)
    {
        while (_attacking)
        {
            target.GetComponent<ILiving>()?.TakeDamage(stats.GetValue(StatType.Attack), stats.Affinity);
            yield return new WaitForSeconds(1f / stats.GetValue(StatType.AttackSpeed));
        }

        _attackCoroutine = null;
    }
}
