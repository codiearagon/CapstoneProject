using UnityEngine;

public interface IActor : IDamageable, IManaUser, IStatusEffectable
{
    public Vector2 GetPosition();
    public Vector2 GetLook();
}