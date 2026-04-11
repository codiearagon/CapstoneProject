using UnityEngine;

public interface IMoveEffectable
{
    public void ApplyKnockback(Vector2 force);
    public void ApplyMoveSpeed(float multiplier);
}