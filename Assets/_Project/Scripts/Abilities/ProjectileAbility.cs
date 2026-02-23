using UnityEngine;

public class ProjectileAbility : IAbilityExecution
{
    private ProjectileProperties _properties;

    public ProjectileAbility(ProjectileProperties properties)
    {
        _properties = properties;
    }

    public void SetMovementBehaviour(ProjectileMovementBehaviour movement)
    {
        _properties.MovementBehaviour = movement;
    }

    public void SetHitBehaviour(ProjectileHitBehaviour hit)
    {
        _properties.HitBehaviour = hit;
    }

    public void Execute(GameObject caster, Ability ability, LayerMask layer)
    {
        IProjectileMovement movement = CreateMovementBehaviour();
        IProjectileHit hit = CreateHitBehaviour();

        GameObject projectile = GameObject.Instantiate(_properties.ProjectilePrefab, caster.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetBehaviour(movement, hit, _properties.TimeToLive, layer);
        projectile.transform.localScale = new Vector3(_properties.Size, _properties.Size, 0);
    }


    // ------ Movement and Hit Factory ------
    private IProjectileMovement CreateMovementBehaviour()
    {
        switch (_properties.MovementBehaviour)
        {
            case ProjectileMovementBehaviour.Straight:
                return new StraightProjectile(_properties);
            default:
                return null;
        }
    }

    private IProjectileHit CreateHitBehaviour()
    {
        switch (_properties.HitBehaviour)
        {
            case ProjectileHitBehaviour.Damage:
                return new DamageOnHitProjectile(_properties);
            case ProjectileHitBehaviour.Piercing:
                return new PiercingProjectile(_properties);
            default:
                return null;
        }
    }
}