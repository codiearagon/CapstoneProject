using UnityEngine;

public interface IDamageable
{
    public bool IsDead();
    public void TakeDamage(float amount, Affinity damageAffinity);
}
