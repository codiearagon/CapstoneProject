using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Ability : MonoBehaviour
{
    public int Level;
    public AbilityProperties Properties;

    [Header("Add upgrades to ability (Sets values)")]
    public List<AbilityProperties> Upgrades;

    private Stats _casterStats;

    private float _cooldownRemaining;
    private IAbilityExecution _execution;
    private Vector2 _direction;
    private float _finalDamage;
    private GameObject _caster;
    private LayerMask _layer;

    public void SetLayer(LayerMask layer)
    {
        // simple if statement works because this will not change
        if (layer == LayerMask.NameToLayer("Character"))
            _layer = LayerMask.NameToLayer("CharacterAttack");
        else if (layer == LayerMask.NameToLayer("Enemy"))
            _layer = LayerMask.NameToLayer("EnemyAttack");
        else
            Logger.Log("Invalid ability layer");
    }

    public void ChangeAffinity(Affinity affinity)
    {
        Properties.Affinity = affinity;
    }

    public void Cast(GameObject caster)
    {
        _caster = caster;
        if (Properties.Toggable)
        {
            if(_execution != null)
            {
                _execution.Stop();
                _execution = null;
                return;
            }

            _execution = CreateAbilityExecution();
            _execution.Execute(caster, this, _layer);

            return;
        }

        if (!_caster.GetComponent<IManaUser>().HasMana(Properties.ManaCost))
            return;

        _caster.GetComponent<IManaUser>().UseMana(Properties.ManaCost);
        
        _execution = CreateAbilityExecution();
        _execution.Execute(caster, this, _layer);
    }

    public void SetRuntimeData(Stats stats, Vector2 direction)
    {
        _casterStats = stats;
        _direction = direction;

        float attackDamage = _casterStats.Attack * Properties.AttackMultiplier;
        float affinityMultiplier = Utility.GetMultiplier(_casterStats, Properties.Affinity);

        _finalDamage = attackDamage * affinityMultiplier;
    }

    // execution factory
    private IAbilityExecution CreateAbilityExecution()
    {
        switch(Properties.Type)
        {
            case AbilityType.Projectile:
                Properties.ProjectileProperties.ApplyRuntimeData(_direction, Properties.Affinity, _finalDamage, _casterStats);
                return new ProjectileAbility(Properties.ProjectileProperties);
            case AbilityType.AOE:
                Properties.AOEProperties.ApplyRuntimeData(_caster, Properties.Affinity, _finalDamage, Properties.ManaCost, _caster.GetComponent<IManaUser>(), _casterStats);
                return new AOEAbility(Properties.AOEProperties);
            default:
                return null;
        }
    }

    public float CooldownRemaining => _cooldownRemaining;
}
