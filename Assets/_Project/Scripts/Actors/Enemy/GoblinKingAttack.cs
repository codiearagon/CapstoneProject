  using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoblinKingAttack : EnemyAttack
{
    [SerializeField] private List<Ability> _defaultAbilities;
    [SerializeField] private List<Ability> _phase2Abilities;
    [SerializeField] private List<Ability> _phase3Abilities;

    [SerializeField] private float _abilityBurst;

    private List<Ability> _runtimeAbilities;
    private GoblinKingBrain _brain;

    private void Awake()
    {
        _runtimeAbilities = new List<Ability>();
        _brain = GetComponent<GoblinKingBrain>();
    }

    private void OnEnable()
    {
        _brain.OnPhaseChange += HandlePhaseChange;
    }

    private void OnDisable()
    {
        _brain.OnPhaseChange -= HandlePhaseChange;
    }

    private void Start()
    {
        AddAbilities(_defaultAbilities);
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

    private void HandlePhaseChange(int phase)
    {
        switch(phase)
        {
            case 2:
                _abilityBurst += 1;
                break;
            case 3:
                _abilityBurst += 1;
                AddAbilities(_phase3Abilities);
                break;
        }
    }

    private void AddAbilities(List<Ability> abilities)
    {
        foreach(Ability ability in abilities)
        {
            Ability runtimeAbility = Instantiate(ability, transform);
            runtimeAbility.SetLayer(gameObject.layer);
            _runtimeAbilities.Add(runtimeAbility);
        }
    }
}
