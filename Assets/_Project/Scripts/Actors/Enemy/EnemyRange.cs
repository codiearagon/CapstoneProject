using System;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    public bool TargetInRange { get; private set; } = false;

    public event Action<bool> InRangeChanged;

    private Enemy _enemy;
    private SpriteRenderer _rangeSprite;
    private CircleCollider2D _rangeCollider;

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
        TargetInRange = true;
        InRangeChanged?.Invoke(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TargetInRange = false;
        InRangeChanged?.Invoke(false);
    }
}