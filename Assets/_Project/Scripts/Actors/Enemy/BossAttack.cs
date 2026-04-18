using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    [SerializeField]
    private List<Ability> _abilities;

    [SerializeField]
    private float _abilityBurst;

    private List<Ability> _runtimeAbilities;

    private SpriteRenderer _rangeSprite;
    private CircleCollider2D _rangeCollider;
    private Enemy _enemy;
    private Rigidbody2D _rb;

    private GameObject _target;
    private bool _playerInRange;
    private Coroutine _attackCoroutine;

    private void Awake()
    {
        _runtimeAbilities = new List<Ability>();
        _rangeCollider = GetComponent<CircleCollider2D>();
        _rangeSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _rb = _enemy.GetComponent<Rigidbody2D>();
        _rangeCollider.radius = _enemy.Stats.AttackRange;

        foreach (Ability ability in _abilities)
        {
            Ability runtimeAbility = Instantiate(ability, transform);
            runtimeAbility.SetLayer(transform.parent.gameObject.layer);
            _runtimeAbilities.Add(runtimeAbility);
        }

        _rangeSprite.color = new Color(1f, 0f, 0f, 0.3f);
        _rangeSprite.transform.localScale = new Vector3(_enemy.Stats.AttackRange * 2.5f, _enemy.Stats.AttackRange * 2.5f, 1f);

        // enemy events
        _enemy.OnDamage += HandleOnDamage;
        _enemy.OnDeath += HandleOnDeath;
    }

    private void OnDestroy()
    {
        _enemy.OnDamage -= HandleOnDamage;
        _enemy.OnDeath -= HandleOnDeath;
    }

    private void HandleOnDamage(float currentHp)
    {

    }

    private void HandleOnDeath()
    {

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

            StartCoroutine(AttackRandomAbility());
            yield return new WaitForSeconds(3f / _enemy.Stats.AttackSpeed);

            if (!_playerInRange)
            {
                _attackCoroutine = null;
                yield break;
            }
        }
    }

    private IEnumerator AttackRandomAbility()
    {
        int randomIdx = Random.Range(0, _runtimeAbilities.Count);
        Ability toUse = _runtimeAbilities[randomIdx];

        for(int i = 0; i < _abilityBurst; i++)
        {
            Vector2 direction = ((Vector2)_target.transform.position - _rb.position).normalized;

            toUse.SetRuntimeData(_enemy.Stats, direction);
            toUse.Cast(transform.parent.gameObject);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
