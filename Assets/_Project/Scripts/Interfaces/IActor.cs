using UnityEngine;

public interface IActor : ILiving, IManaUser, IStatusEffectable, IStatEffectable
{
    public Vector2 GetPosition();
    public Vector2 GetLook();
}