using System;
using UnityEngine;

[Serializable]
public class StraightProjectile : IProjectileMovement
{
    // To be called in fixed delta time
    public void Move(Projectile projectile, Rigidbody2D projectileRb)
    {
        projectileRb.MovePosition(projectileRb.position + projectile.Direction * projectile.Speed * Time.fixedDeltaTime);
    }
}
