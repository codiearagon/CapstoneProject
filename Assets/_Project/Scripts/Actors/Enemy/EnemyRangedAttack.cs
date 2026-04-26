using System.Collections;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttack
{
    [SerializeField]
    private Ability _ability;
    private Ability _runtimeAbility;

    private void Awake()
    {
        _runtimeAbility = Instantiate(_ability, transform);
        _runtimeAbility.SetLayer(gameObject.layer);
    }

    protected override IEnumerator AttackLoop(Stats stats, GameObject target)
    {
        while(_attacking)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;

            _runtimeAbility.SetRuntimeData(stats, direction);
            _runtimeAbility.Cast(gameObject);

            yield return new WaitForSeconds(1f / stats.GetValue(StatType.AttackSpeed));
        }

        _attackCoroutine = null;
    }
}
