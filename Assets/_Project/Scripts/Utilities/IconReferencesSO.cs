using UnityEngine;

[CreateAssetMenu()]
public class IconReferencesSO : ScriptableObject
{
    public Sprite MaxHp;
    public Sprite HpRegen;
    public Sprite MaxMana;
    public Sprite ManaRegen;
    public Sprite MovementSpeed;
    public Sprite Attack;
    public Sprite AttackSpeed;
    public Sprite Defense;
    public Sprite Fire;
    public Sprite Water;
    public Sprite Air;
    public Sprite Earth;
    public Sprite Dark;
    public Sprite Light;

    public Sprite GetIcon(StatType type)
    {
        switch(type)
        {
            case StatType.MaxHp: return MaxHp;
            case StatType.HpRegenRate:return HpRegen;
            case StatType.MaxMana: return MaxMana;
            case StatType.ManaRegenRate: return ManaRegen;
            case StatType.MovementSpeed: return MovementSpeed;
            case StatType.Attack: return Attack;
            case StatType.AttackSpeed: return AttackSpeed;
            case StatType.Defense: return Defense;
            case StatType.FireMultiplier: return Fire;
            case StatType.WaterMultiplier: return Water;
            case StatType.AirMultiplier: return Air;
            case StatType.EarthMultiplier: return Earth;
            case StatType.DarkMultiplier: return Dark;
            case StatType.LightMultiplier: return Light;
            default: return null;
        }
    }

    public Sprite GetIcon(Affinity affinity)
    {
        switch(affinity)
        {
            case Affinity.Fire: return Fire;
            case Affinity.Water: return Water;
            case Affinity.Air: return Air;
            case Affinity.Earth: return Earth;
            case Affinity.Dark: return Dark;
            case Affinity.Light: return Light;
            default: return null;
        }
    }
}