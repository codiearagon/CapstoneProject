using UnityEngine;

public enum ProjectileHitBehaviour
{
    Damage,
    Piercing,
    Knockback,
}

public interface IProjectileHit
{
    public void OnHit(Projectile projectile, Collider2D other);
}