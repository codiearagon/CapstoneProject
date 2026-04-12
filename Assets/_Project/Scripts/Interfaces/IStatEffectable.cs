using UnityEngine;

public interface IStatEffectable
{
    public void ApplyKnockback(Vector2 force);
    public void ApplyMoveSpeed(float multiplier);
    public void ApplyStatChange(StatType stat, float multiplier);
}