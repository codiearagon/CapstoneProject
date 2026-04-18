using System.Collections;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField]
    private Ability _ability;

    private Ability _runtimeAbility;

    private SpriteRenderer _rangeSprite;
    private CircleCollider2D _rangeCollider;
    private Enemy _enemy;
    private Rigidbody2D _rb;

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
        _rb = _enemy.GetComponent<Rigidbody2D>();
        _rangeCollider.radius = _enemy.Stats.AttackRange;

        _runtimeAbility = Instantiate(_ability, transform);
        _runtimeAbility.SetLayer(transform.parent.gameObject.layer);

        _rangeSprite.color = new Color(1f, 0f, 0f, 0.3f);
        _rangeSprite.transform.localScale = new Vector3(_enemy.Stats.AttackRange * 2.5f, _enemy.Stats.AttackRange * 2.5f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _target = collision.gameObject;
        _playerInRange = true;
        _enemy.PlayerInRange(true);

        if (_attackCoroutine == null)
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
            Logger.Log("Ranged enemy attacked");
            Vector2 direction = ((Vector2)_target.transform.position - _rb.position).normalized;

            _runtimeAbility.SetRuntimeData(_enemy.Stats, direction);
            _runtimeAbility.Cast(transform.parent.gameObject);

            yield return new WaitForSeconds(1f / _enemy.Stats.AttackSpeed);

            if (!_playerInRange)
            {
                _attackCoroutine = null;
                yield break;
            }
        }
    }
}
