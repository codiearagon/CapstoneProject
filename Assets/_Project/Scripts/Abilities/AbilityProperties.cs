using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityProperties
{
    [Header("Details")]
    public string AbilityName;
    public string Identifier;
    public Sprite Icon;
    [TextArea] public string Description;

    [Header("Basic Properties")]
    public Affinity Affinity;
    public float AttackMultiplier;
    public float ManaCost;
    public float CooldownTime;
    public bool Toggable;
    public AbilityType Type;

    [Space]
    public ProjectileProperties ProjectileProperties;
    public AOEProperties AOEProperties;

    public void ApplyProperties(AbilityProperties properties)
    {
        AbilityName = properties.AbilityName;
        Icon = properties.Icon;
        Affinity = properties.Affinity;
        AttackMultiplier = properties.AttackMultiplier;
        ManaCost = properties.ManaCost;
        CooldownTime = properties.CooldownTime;
        Toggable = properties.Toggable;
        Type = properties.Type;

        ProjectileProperties.ApplyProperties(properties.ProjectileProperties);
        AOEProperties.ApplyProperties(properties.AOEProperties);
    }
}
