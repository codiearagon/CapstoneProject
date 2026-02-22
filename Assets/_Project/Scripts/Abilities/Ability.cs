using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public AbilityProperties Properties;

    private IAbilityExecution _execution;
    private Vector2 _direction;
    private float _finalDamage;
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

    public void Cast(GameObject caster)
    {
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
                Properties.ProjectileProperties.SetDamage(_finalDamage);
                Properties.ProjectileProperties.SetDirection(_direction);
                Properties.ProjectileProperties.SetAffinity(Properties.Affinity);
                return new ProjectileAbility(Properties.ProjectileProperties);
            default:
                return null;
        }
    }
}
