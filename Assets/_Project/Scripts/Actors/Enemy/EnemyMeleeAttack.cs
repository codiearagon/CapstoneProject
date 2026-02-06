using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    private CircleCollider2D _rangeCollider;
    private Enemy _enemy;

    private GameObject _target;
    private bool _playerInRange;

    private void Awake()
    {
        _rangeCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _rangeCollider.radius = _enemy.Stats.AttackRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _target = collision.gameObject;
        _playerInRange = true;
        StartCoroutine(Attack());
        Logger.Log("Player in range");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _target = collision.gameObject;
        _playerInRange = false;
        Logger.Log("Player out of range");
    }

    private IEnumerator Attack()
    {
        Logger.Log("Attack coroutine started");

        while (_playerInRange)
        {
            _target.GetComponent<IDamageable>()?.TakeDamage(_enemy.Stats.Attack, _enemy.Stats.Affinity);
            yield return new WaitForSeconds(1 / _enemy.Stats.AttackSpeed);
        }
    }

    // Editor stuff
    private void OnDrawGizmos()
    {
        if (_enemy == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemy.Stats.AttackRange);
    }
}
