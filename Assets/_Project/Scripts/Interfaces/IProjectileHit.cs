using UnityEngine;

public enum ProjectileHitBehaviour
{
    Damage
}

public interface IProjectileHit
{
    public void OnHit(Projectile projectile, Collider2D other);
}