using UnityEngine;

public interface IStatEffectable
{
    public void AddStatModifier(StatModifier statModifier);
    public void RemoveStatModifiers(object source);
}