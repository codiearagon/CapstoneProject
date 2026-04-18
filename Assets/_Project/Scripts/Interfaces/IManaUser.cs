
public interface IManaUser
{
    public bool HasMana(float amount);
    public void UseMana(float amount);
    public void GainMana(float amount);
    public void FullMana();
}