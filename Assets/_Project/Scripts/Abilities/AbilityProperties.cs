using UnityEngine;

[System.Serializable]
public class AbilityProperties
{
    // details
    public string AbilityName;
    public Sprite Icon;
    public string Description;

    // basic properties
    public Affinity Affinity;
    public float AttackMultiplier;
    public float ManaCost;
    public float CooldownTime;
    public float CooldownRemaining;
    public bool AlwaysActive;
    public AbilityType Type;

    // projectile related fields
    public ProjectileProperties ProjectileProperties;
}
