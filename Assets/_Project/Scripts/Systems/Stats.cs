// Serves as realtime stats container
public class Stats
{
    public int MaxHp;
    public int CurrentHp;
    public int MovementSpeed;
    public int Attack;
    public int AttackSpeed;
    public int Defense;

    public void InitializeStats(BaseSO baseSO)
    {
        if (baseSO == null)
            Logger.Log("BaseData is null");

        MaxHp = baseSO.Hp;
        CurrentHp = baseSO.Hp;
        MovementSpeed = baseSO.MovementSpeed;
        Attack = baseSO.Attack;
        AttackSpeed = baseSO.AttackSpeed;
        Defense = baseSO.Defense;
    }
}
