using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class EnemyMeleeAttack : MonoBehaviour
{
    private SpriteRenderer _rangeSprite;
    private CircleCollider2D _rangeCollider;
    private Enemy _enemy;

    private GameObject _target;
    private bool _playerInRange;
    private Coroutine _attackCoroutine;

    private void Awake()
    {
        _rangeCollider = GetComponent<CircleCollider2D>();
        _rangeSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _rangeCollider.radius = _enemy.Stats.AttackRange;

        _rangeSprite.color = new Color(1f, 0f, 0f, 0.3f);
        _rangeSprite.transform.localScale = new Vector3(_enemy.Stats.AttackRange * 2.5f, _enemy.Stats.AttackRange * 2.5f, 1f);
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
