using UnityEngine;

public class ProjectileAbility : IAbilityExecution
{
    private ProjectileMovementBehaviour _movementBehaviour;
    private ProjectileHitBehaviour _hitBehaviour;

    private ProjectileProperties _properties;

    public ProjectileAbility(ProjectileProperties properties)
    {
        _properties = properties;
    }

    public void SetMovementBehaviour(ProjectileMovementBehaviour movement)
    {
        _movementBehaviour = movement;
    }

    public void SetHitBehaviour(ProjectileHitBehaviour hit)
    {
        _hitBehaviour = hit;
    }

    public void Execute(GameObject caster, Ability ability, LayerMask layer)
    {
        IProjectileMovement movement = CreateMovementBehaviour(_movementBehaviour);
        IProjectileHit hit = CreateHitBehaviour(_hitBehaviour);

        GameObject projectile = GameObject.Instantiate(_properties.ProjectilePrefab, caster.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetBehaviour(movement, hit, _properties.TimeToLive, layer);
    }


    // ------ Movement and Hit Factory ------
    private IProjectileMovement CreateMovementBehaviour(ProjectileMovementBehaviour movement)
    {
        switch (movement)
        {
            case ProjectileMovementBehaviour.Straight:
                return new StraightProjectile(_properties);
            default:
                return null;
        }
    }

    private IProjectileHit CreateHitBehaviour(ProjectileHitBehaviour hit)
    {
        switch (hit)
        {
            case ProjectileHitBehaviour.Damage:
                return new DamageOnHitProjectile(_properties);
            default:
                return null;
        }
    }
}