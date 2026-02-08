using UnityEngine;

public class ProjectileAbility : Ability
{
    [Header("Projectile Properties")]
    public GameObject ProjectilePrefab;
    public float TimeToLive;

    private IProjectileMovement _movement;
    private IProjectileHit _hit;

    private Vector2 _direction;

    public void SetDamage(float damage)
    {
        FinalDamage = damage;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void SetBehaviour(IProjectileMovement movement, IProjectileHit hit)
    {
        _movement = movement;
        _hit = hit;
    }

    public override void Cast(GameObject caster, LayerMask layer)
    {
        GameObject projectile = Instantiate(ProjectilePrefab, caster.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetData(FinalDamage, Affinity, _direction, layer);
        projectile.GetComponent<Projectile>().SetBehaviour(_movement, _hit);
    }
}
