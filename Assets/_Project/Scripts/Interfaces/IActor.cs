using UnityEngine;

public interface IActor : ILiving, IManaUser, IStatusEffectable, IStatEffectable, ICCable
{
    public Vector2 GetPosition();
    public Vector2 GetLook();
}