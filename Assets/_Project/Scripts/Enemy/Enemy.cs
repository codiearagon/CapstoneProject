using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyBaseSO BaseStats { get; private set; }
    public Stats Stats { get; private set; }

    private void Awake()
    {
        Stats.InitializeStats(BaseStats);
    }

    private void Update()
    {
        
    }
    public void TakeDamage(int amount)
    {
        Stats.CurrentHp = Mathf.Clamp(Stats.CurrentHp - amount, 0, Stats.MaxHp);
    }
}
