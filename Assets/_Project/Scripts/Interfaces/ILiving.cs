using UnityEngine;

public interface ILiving
{
    public bool IsDead();
    public void TakeDamage(float amount, Affinity damageAffinity);
    public void Heal(float flatAmount);
    public void HealPercent(float percentage);
    public void FullHeal();
}
