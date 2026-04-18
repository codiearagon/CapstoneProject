using UnityEngine;

public interface ILiving
{
    public bool IsDead();
    public void TakeDamage(float amount, Affinity damageAffinity);
    public void Heal(float amount);
    public void FullHeal();
}
