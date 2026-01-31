using UnityEngine;

[RequireComponent(typeof(ActorMovement))]
public class Enemy : Actor
{
    [field: SerializeField]
    public EnemyBaseSO BaseData { get; private set; }


    private void Awake()
    {
        InputData = new ActorInputData();
        InitializeActor(BaseData);
    }

    // testing purposes
    private void Update()
    {
        InputData.MoveValue = GameObject.FindGameObjectWithTag("Character").transform.position.normalized;
        InputData.LookValue = Vector2.zero;
        UpdateInputData(InputData);
    }

    public override void InitializeActor(ActorBaseSO baseData)
    {
        Stats = new Stats();
        Stats.InitializeStats(BaseData);
    }

    public override void UpdateInputData(ActorInputData inputData)
    {
        InputData = inputData;
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
