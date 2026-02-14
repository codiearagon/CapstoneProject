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

    [Header("Base Properties")]
    public Affinity Affinity;
    public float HpMultiplier;
    public float AttackMultiplier;
    public float DefenseMultiplier;
    public float ManaCost;
    public float CooldownTime;
    public bool AlwaysActive;

    public abstract void Cast(GameObject caster, LayerMask layer);
}
