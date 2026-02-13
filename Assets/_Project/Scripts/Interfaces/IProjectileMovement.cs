using UnityEngine;

public enum ProjectileMovementBehaviour
{
    Straight
}

public interface IProjectileMovement
{
    public void Move(Projectile projectile, Rigidbody2D projectileRb);
}