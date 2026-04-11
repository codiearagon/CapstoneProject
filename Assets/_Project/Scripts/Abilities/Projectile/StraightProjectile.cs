using System;
using UnityEngine;

public class StraightProjectile : IProjectileMovement
{
    private float _speed;
    private Vector2 _direction;
    private bool _rotating;

    public StraightProjectile(ProjectileProperties _projectileProperties)
    {
        _speed = _projectileProperties.Speed;
        _direction = _projectileProperties.Direction;
        _rotating = _projectileProperties.Rotating;
    }

    // To be called in fixed delta time
    public void Move(Projectile projectile, Rigidbody2D projectileRb)
    {
        projectileRb.MovePosition(projectileRb.position + _direction * _speed * Time.fixedDeltaTime);

        if(_rotating)
            projectileRb.MoveRotation(Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg);
    }
}
