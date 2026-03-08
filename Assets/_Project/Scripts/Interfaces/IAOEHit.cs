using UnityEngine;

public enum AOEHitBehaviour
{
    Damage,
}

public interface IAOEHit
{
    public void OnHit(AOE aoe, Collider2D other);
}