using UnityEngine;
using System.Collections;

public enum StatusEffect
{
    None,
    Slow,
    Root,
    Burn,
    Poison,
    Regen,
}

public interface IStatusEffect
{
    public void Apply(Collider2D target);
    public void Remove(Collider2D target);
    public IEnumerator Tick(Collider2D target);
}
