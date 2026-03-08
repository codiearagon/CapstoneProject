using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public int Level;
    public AbilityProperties Properties;

    [Header("Add upgrades to ability (Sets values)")]
    public List<AbilityProperties> Upgrades;

    private float _cooldownRemaining;
    private IAbilityExecution _execution;
    private Vector2 _direction;
    private float _finalDamage;
    private Coroutine _manaDrain;
    private LayerMask _layer;

    private bool UseMana(GameObject caster)
    {
        if (!caster.GetComponent<IManaUser>().HasMana(Properties.ManaCost))
            return false;

        caster.GetComponent<IManaUser>().UseMana(Properties.ManaCost);
        return true;
    }

    private IEnumerator ConstantManaDrain(GameObject caster)
    {
        while (true)
        {
            if (!UseMana(caster))
                continue;

            yield return new WaitForSeconds(2f);

            if(_execution == null)
            {
                _manaDrain = null;
                yield break;
            }
        }
    }

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

            if (_manaDrain == null)
                _manaDrain = StartCoroutine(ConstantManaDrain(caster));

            return;
        }

        if (!UseMana(caster))
            return;

        _execution = CreateAbilityExecution();
        _execution.Execute(caster, this, _layer);
    }

    public void SetRuntimeData(float damage, Vector2 direction)
    {
        _finalDamage = damage;
        _direction = direction;
    }

    // execution factory
    private IAbilityExecution CreateAbilityExecution()
    {
        switch(Properties.Type)
        {
            case AbilityType.Projectile:
                Properties.ProjectileProperties.ApplyRuntimeData(_direction, Properties.Affinity, _finalDamage);
                return new ProjectileAbility(Properties.ProjectileProperties);
            case AbilityType.AOE:
                Properties.AOEProperties.ApplyRuntimeData(Properties.Affinity, _finalDamage);
                return new AOEAbility(Properties.AOEProperties);
            default:
                return null;
        }
    }

    public float CooldownRemaining => _cooldownRemaining;
}
