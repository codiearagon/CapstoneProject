using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyStats _stats;
    
    private void Awake()
    {
        _stats = new EnemyStats();

        _stats.CurrentHp = _stats.MaxHp;
    }

    public void TakeDamage(int amount)
    {
        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - amount, 0, _stats.MaxHp);
    }
}
