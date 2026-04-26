using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour
{
    protected Enemy _enemy;
    protected EnemyMovement _movement;
    protected EnemyRange _range;
    protected EnemyAttack _attack;

    protected GameObject _target;

    protected virtual void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _movement = GetComponent<EnemyMovement>();
        _range = GetComponentInChildren<EnemyRange>();
        _attack = GetComponent<EnemyAttack>();
    }

    protected virtual void OnEnable()
    {
        _range.InRangeChanged += HandleInRangeChanged;
    }

    protected virtual void OnDisable()
    {
        _range.InRangeChanged -= HandleInRangeChanged;
    }

    protected virtual void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Character");

        _movement.SetTarget(_target);
        _enemy.SetTarget(_target);

        RecalculateActions();
    }

    protected virtual void HandleInRangeChanged(bool targetInRange)
    {
        RecalculateActions();
    }

    protected virtual void RecalculateActions()
    {
        if (_range.TargetInRange)
        {
            _movement.StopMoving();
            _attack.StartAttacking(_enemy.Stats, _target);
        }
        else
        {
            _movement.StartMoving();
            _attack.StopAttacking();
        }
    }
}