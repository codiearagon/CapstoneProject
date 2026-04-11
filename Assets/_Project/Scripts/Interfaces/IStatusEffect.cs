using UnityEngine;
using System.Collections;

public enum StatusEffect
{
    None,
    Slow,
    Root,
    Dot,
    Regen,
}

public interface IStatusEffect
{
    public void ApplyEffect(Collider2D target);
    public void Refresh();
    public void RemoveEffect(Collider2D target);
    public IEnumerator Tick(Collider2D target);
}
