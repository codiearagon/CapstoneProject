public interface IStatusEffectable
{
    public void ApplyEffect(IStatusEffect effect);
    public void RemoveEffect(IStatusEffect effect);
}