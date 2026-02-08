using UnityEngine;

public interface IProjectileHit
{
    public void OnHit(Projectile projectile, Collider2D other);
}
