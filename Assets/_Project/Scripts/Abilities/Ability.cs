using System;
using UnityEngine;

public enum AbilityState
{
    Cooldown,
    Ready,
    Active
}

public abstract class Ability : MonoBehaviour
{
    [Header("Details")]
    public string AbilityName;
    public Sprite Icon;

    [TextArea] 
    public string Description;

    [Header("Base Properties")]
    public Affinity Affinity;
    public float AttackMultiplier;
    public float ManaCost;
    public float CooldownTime;
    public bool AlwaysActive;

    public abstract void Cast(GameObject caster, LayerMask layer);
}
