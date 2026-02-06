using UnityEngine;

public interface IAttackBehaviour
{
    public void Attack(float damage, Vector2 direction, Affinity affinity);
}
