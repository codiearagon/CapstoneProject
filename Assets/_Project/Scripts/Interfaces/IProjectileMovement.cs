using UnityEngine;

public interface IProjectileMovement
{
    public void Move(Projectile projectile, Rigidbody2D projectileRb);
}
