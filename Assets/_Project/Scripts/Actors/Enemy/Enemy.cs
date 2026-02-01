using UnityEngine;

[RequireComponent(typeof(ActorMovement))]
public class Enemy : Actor
{
    [field: SerializeField]
    public EnemyBaseSO BaseData { get; private set; }
    
    // Enemy specific stats
    public float AggroRadius { get; private set; }
    public float AttackRange { get; private set; }

    private void Awake()
    {
        InputData = new ActorInputData();
        InitializeActor(BaseData);

        AggroRadius = BaseData.AggroRadius;
        AttackRange = BaseData.AttackRange;
    }

    public override void InitializeActor(ActorBaseSO baseData)
    {
        Stats = new Stats();
        Stats.InitializeStats(BaseData);
    }

    public override void UpdateInputData(Vector2 moveValue, Vector2 lookValue)
    {
        InputData.MoveValue = moveValue;
        InputData.LookValue = lookValue;
    }

    public override void TakeDamage(int amount)
    {
        Stats.CurrentHp = Mathf.Clamp(Stats.CurrentHp - amount, 0, Stats.MaxHp);
    }

    public void SetBaseData(EnemyBaseSO baseData)
    {
        BaseData = baseData;
    }
}
