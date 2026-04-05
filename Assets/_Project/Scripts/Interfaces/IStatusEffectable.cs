public interface IStatusEffectable
{
    public void Apply(IStatusEffect effect);
    public void Remove(IStatusEffect effect);
}