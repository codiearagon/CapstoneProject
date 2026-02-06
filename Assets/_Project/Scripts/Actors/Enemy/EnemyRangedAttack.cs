using System.Collections;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    private CircleCollider2D _rangeCollider;
    private Enemy _enemy;

    private bool _playerInRange;

    private void Awake()
    {
        _rangeCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInRange = true;
        StartCoroutine(Attack());
        Logger.Log("Player in range");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerInRange = false;
        Logger.Log("Player out of range");
    }

    private IEnumerator Attack()
    {
        Logger.Log("Attack coroutine started");

        while (_playerInRange)
        {
            yield return new WaitForSeconds(1 / _enemy.Stats.AttackSpeed);
        }
    }

    // Editor stuff
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemy.Stats.AttackRange);
    }
}
