using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoblinBossAttack : EnemyAttack
{
    [SerializeField]
    private List<Ability> _abilities;
    private List<Ability> _runtimeAbilities;

    [SerializeField]
    private float _abilityBurst;

    private void Awake()
    {
        _runtimeAbilities = new List<Ability>();
    }

    private void Start()
    {
        foreach (Ability ability in _abilities)
        {
            Ability runtimeAbility = Instantiate(ability, transform);
            runtimeAbility.SetLayer(gameObject.layer);
            _runtimeAbilities.Add(runtimeAbility);
        }
    }

    private IEnumerator AttackRandomAbility(Stats stats, GameObject target)
    {
        int randomIdx = Random.Range(0, _runtimeAbilities.Count);
        Ability toUse = _runtimeAbilities[randomIdx];

        for(int i = 0; i < _abilityBurst; i++)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;

            toUse.SetRuntimeData(stats, direction);
            toUse.Cast(gameObject);
            yield return new WaitForSeconds(0.3f);
        }
    }

    protected override IEnumerator AttackLoop(Stats stats, GameObject target)
    {
        while (_attacking)
        {
            StartCoroutine(AttackRandomAbility(stats, target));
            yield return new WaitForSeconds(3f / stats.AttackSpeed);
        }

        _attackCoroutine = null;
    }
}
