using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    private CircleCollider2D _rangeCollider;
    private Enemy _enemy;

    private GameObject _target;
    private bool _playerInRange;
    private Coroutine _attackCoroutine;

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

        if(_attackCoroutine == null)
            _attackCoroutine = StartCoroutine(Attack());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _target = collision.gameObject;
        _enemy.PlayerInRange(false);
        _playerInRange = false;
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / _enemy.Stats.AttackSpeed);

            if(!_enemy.IsPaused)
                _target.GetComponent<IDamageable>()?.TakeDamage(_enemy.Stats.Attack, _enemy.Stats.Affinity);

            if(!_playerInRange)
            {
                _attackCoroutine = null;
                yield break;
            }
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
