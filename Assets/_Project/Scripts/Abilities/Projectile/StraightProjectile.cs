using System;
using UnityEngine;

public class StraightProjectile : IProjectileMovement
{
    private float _speed;
    private Vector2 _direction;

    public StraightProjectile(float speed, Vector2 direction)
    {
        _speed = speed;
        _direction = direction;
    }

    // To be called in fixed delta time
    public void Move(Projectile projectile, Rigidbody2D projectileRb)
    {
        projectileRb.MovePosition(projectileRb.position + _direction * _speed * Time.fixedDeltaTime);
    }
}
