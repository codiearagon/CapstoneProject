using System;
using UnityEngine;

public class StraightProjectile : IProjectileMovement
{
    private float _speed;
    private Vector2 _direction;

    public StraightProjectile(ProjectileProperties _projectileProperties)
    {
        _speed = _projectileProperties.Speed;
        _direction = _projectileProperties.Direction;
    }

    // To be called in fixed delta time
    public void Move(Projectile projectile, Rigidbody2D projectileRb)
    {
        projectileRb.MovePosition(projectileRb.position + _direction * _speed * Time.fixedDeltaTime);
    }
}
