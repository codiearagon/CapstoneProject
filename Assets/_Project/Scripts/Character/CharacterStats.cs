using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [field: SerializeField]
    public CharacterBaseSO BaseData { get; private set; }

    // realtime stats
    public int MaxHp { get; private set; }
    public int CurrentHp { get; private set; }
    public int MovementSpeed { get; private set; }
    public int Attack { get; private set; }
    public int AttackSpeed { get; private set; }
    public int Defense { get; private set; }

    private void Start()
    {
        MaxHp = BaseData.Hp;
        CurrentHp = BaseData.Hp;
        MovementSpeed = BaseData.MovementSpeed;
        Attack = BaseData.Attack;
        AttackSpeed = BaseData.AttackSpeed;
        Defense = BaseData.Defense;
    }

    private void Update()
    {
        
    }
}
