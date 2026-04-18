using UnityEngine;

public interface IActor : IDamageable, IManaUser, IStatusEffectable, IStatEffectable
{
    public Vector2 GetPosition();
    public Vector2 GetLook();
}