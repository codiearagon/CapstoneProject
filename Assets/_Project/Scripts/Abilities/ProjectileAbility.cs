using UnityEngine;

public class ProjectileAbility : Ability
{
    [Header("Core Projectile Properties")]
    public GameObject ProjectilePrefab;
    public float Speed;
    public float TimeToLive;

    [Header("Explosion")]
    public float ExplosionRadius;

    private IProjectileMovement _movement;
    private IProjectileHit _hit;

    private Vector2 _direction;
    private float _finalDamage;

    private void Awake()
    {

    }

    public void SetData(float damage, Vector2 direction)
    {
        _finalDamage = damage;
        _direction = direction;
    }

    public void ChangeMovementBehaviour(IProjectileMovement newMovement)
    {
        _movement = newMovement;
    }

    public void ChangeHitBehaviour(IProjectileHit newHit)
    {
        _hit = newHit;
    }

    public override void Cast(GameObject caster, LayerMask layer)
    {
        _movement = new StraightProjectile(Speed, _direction);
        _hit = new DamageOnHitProjectile(_finalDamage, Affinity);

        GameObject projectile = Instantiate(ProjectilePrefab, caster.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetBehaviour(_movement, _hit, TimeToLive, layer);
    }

    private IProjectileMovement CreateMovementBehaviour()
    {
        Logger.Log(_movement.GetType().FullName);

        if (_movement is StraightProjectile)
        {
            Logger.Log("Creating movement behaviour");
            return new StraightProjectile(Speed, _direction);
        }
        else
            return null;
    }

    private IProjectileHit CreateHitBehaviour()
    {
        Logger.Log(_hit.GetType().FullName);

        if (_hit is DamageOnHitProjectile)
        {
            Logger.Log("Creating hit behaviour");
            return new DamageOnHitProjectile(_finalDamage, Affinity);
        }
        else
            return null;
    }
}