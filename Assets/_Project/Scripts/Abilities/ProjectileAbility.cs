using UnityEngine;

public class ProjectileAbility : Ability
{
    [Header("Core Projectile Properties")]
    public GameObject ProjectilePrefab;
    public float Speed;
    public float TimeToLive;

    [SerializeField] private ProjectileMovementBehaviour _movementBehaviour;
    [SerializeField] private ProjectileHitBehaviour _hitBehaviour;

    [Header("Miscellaneous Properties")]
    public float ExplosionRadius;

    private Vector2 _direction;
    private float _finalDamage;

    public void SetData(float damage, Vector2 direction)
    {
        _finalDamage = damage;
        _direction = direction;
    }

    public void SetMovementBehaviour(ProjectileMovementBehaviour movement)
    {
        _movementBehaviour = movement;
    }

    public void SetHitBehaviour(ProjectileHitBehaviour hit)
    {
        _hitBehaviour = hit;
    }

    public override void Cast(GameObject caster, LayerMask layer)
    {
        IProjectileMovement movement = CreateMovementBehaviour(_movementBehaviour);
        IProjectileHit hit = CreateHitBehaviour(_hitBehaviour);

        GameObject projectile = Instantiate(ProjectilePrefab, caster.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetBehaviour(movement, hit, TimeToLive, layer);
    }


    // ------ Movement and Hit Factory ------
    private IProjectileMovement CreateMovementBehaviour(ProjectileMovementBehaviour movement)
    {
        switch (movement)
        {
            case ProjectileMovementBehaviour.Straight:
                return new StraightProjectile(Speed, _direction);
            default:
                return null;
        }
    }

    private IProjectileHit CreateHitBehaviour(ProjectileHitBehaviour hit)
    {
        switch (hit)
        {
            case ProjectileHitBehaviour.Damage:
                return new DamageOnHitProjectile(_finalDamage, Affinity);
            default:
                return null;
        }
    }
}