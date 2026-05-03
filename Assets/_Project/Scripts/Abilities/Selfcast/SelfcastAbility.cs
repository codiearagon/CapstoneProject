using System.Collections.Generic;
using UnityEngine;

public class SelfcastAbility : IAbilityExecution
{
    private SelfcastProperties _properties;
    private List<IStatusEffect> _statusEffects = new List<IStatusEffect>();

    public void Execute(GameObject caster, Ability ability, LayerMask layer)
    {
        foreach (StatusEffectProperties statProps in _properties.StatusEffectProperties)
        {
            Logger.Log("Created status effect");
            _statusEffects.Add(StatusEffectFactory.CreateStatusEffect(statProps));
        }

        foreach(IStatusEffect statusEffect in _statusEffects)
        {
            caster.GetComponent<IStatusEffectable>().ApplyEffect(statusEffect);
        }
    }

    public void Stop() {}
}