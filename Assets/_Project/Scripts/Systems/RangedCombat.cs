using UnityEngine;

public class RangedCombat : MonoBehaviour, IAttackBehaviour
{
    [SerializeField]
    private GameObject _projectilePrefab;

    public void Attack(float damage, Vector2 direction, Affinity affinity)
    {
        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetProperties(damage, direction, affinity, gameObject.layer);
    }
}
