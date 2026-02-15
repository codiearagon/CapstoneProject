using UnityEngine;

public enum AbilityType
{
    Projectile,
    AOE,
    Melee,
    Self
}

public interface IAbilityExecution
{
    public void Execute(GameObject caster, Ability ability, LayerMask layer);
}