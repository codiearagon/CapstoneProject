using System.Collections;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    protected bool _attacking;
    protected Coroutine _attackCoroutine;

    protected abstract IEnumerator AttackLoop(Stats stats, GameObject target);

    public virtual void StartAttacking(Stats stats, GameObject target)
    {
        if (_attackCoroutine != null) return;

        _attacking = true;
        _attackCoroutine = StartCoroutine(AttackLoop(stats, target));
    }

    public virtual void StopAttacking()
    {
        if (_attackCoroutine == null) return;

        _attacking = false;
    }
}