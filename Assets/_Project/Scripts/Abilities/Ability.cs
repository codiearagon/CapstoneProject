using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public enum AbilityState
{
    Cooldown,
    Ready,
    Active
}

public class Ability : MonoBehaviour
{
    // details
    public string AbilityName;
    public Sprite Icon;
    public string Description;

    // basic properties
    public Affinity Affinity;
    public float AttackMultiplier;
    public float ManaCost;
    public float CooldownTime;
    public bool AlwaysActive;
    public AbilityType Type;

    // projectile related fields
    public ProjectileProperties ProjectileProperties;

    private IAbilityExecution _execution;
    private Vector2 _direction;
    private float _finalDamage;
    private LayerMask _layer;

    private void Awake()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Character"))
            _layer = LayerMask.NameToLayer("CharacterAttack");
        else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            _layer = LayerMask.NameToLayer("EnemyAttack");
        else
            Logger.Log("Invalid ability layer");
    }

    public void Cast(GameObject caster)
    {
        _execution = CreateAbilityExecution();
    }

    public void SetRuntimeData(float damage, Vector2 direction)
    {
        _finalDamage = damage;
        _direction = direction;
    }

    private IAbilityExecution CreateAbilityExecution()
    {
        switch(Type)
        {
            case AbilityType.Projectile:
                ProjectileProperties.SetAffinity(Affinity);
                return new ProjectileAbility(ProjectileProperties);
            default:
                return null;
        }
    }
}
