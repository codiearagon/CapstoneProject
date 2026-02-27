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
        _enemy.PlayerInRange(true);
        StartCoroutine(Attack());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _target = collision.gameObject;
        _enemy.PlayerInRange(false);
        _playerInRange = false;
    }

    private IEnumerator Attack()
    {
        while (_playerInRange)
        {
            yield return new WaitForSeconds(1 / _enemy.Stats.AttackSpeed);
            if(!_enemy.IsPaused)
                _target.GetComponent<IDamageable>()?.TakeDamage(_enemy.Stats.Attack, _enemy.Stats.Affinity);
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
