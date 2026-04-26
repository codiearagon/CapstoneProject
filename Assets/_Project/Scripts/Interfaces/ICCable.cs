using UnityEngine;

public interface ICCable
{
    public void ApplyKnockback(Vector2 force);
    public void ApplySilence();
    public void ApplyDisarm();
    public void ApplyStun();
}