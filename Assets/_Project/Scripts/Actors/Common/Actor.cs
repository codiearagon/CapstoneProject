using UnityEngine;

public abstract class Actor : MonoBehaviour, IDamageable
{
    public ActorInputData InputData { get; protected set; }
    public Stats Stats { get; protected set; }

    public abstract void InitializeActor(ActorBaseSO baseData);
    public abstract void UpdateInputData(ActorInputData inputData);
    public abstract void TakeDamage(int amount);
}
