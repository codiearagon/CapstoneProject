using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Enemy _enemy;
    private Rigidbody2D _rb;

    private GameObject _target;
    private Rigidbody2D _targetRb;

    private bool _isKnockedback;
    private bool _moving;
    private Vector2 _lookValue;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moving = false;
        _isKnockedback = false;
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        if (_isKnockedback)
            return;

        float speed = Mathf.Min(_enemy.Stats.GetValue(StatType.MovementSpeed), 120);

        if (_target != null && _moving)
        {
            _rb.MovePosition(Vector2.MoveTowards(_rb.position, _targetRb.position, (speed / 10) * Time.fixedDeltaTime));
            _lookValue = (_targetRb.position - _rb.position).normalized;
        }
    }

    private IEnumerator Knockbacked()
    {
        yield return new WaitForSeconds(1f);
        _isKnockedback = false;
        _rb.linearVelocity = Vector3.zero;
    }

    public void StartMoving()
    {
        _moving = true;
    }

    public void StopMoving()
    {
        _moving = false;
    }

    public void ApplyKnockback(Vector2 force)
    {
        _rb.AddForce(force, ForceMode2D.Impulse);
        _isKnockedback = true;
        StartCoroutine(Knockbacked());
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        _targetRb = target.GetComponent<Rigidbody2D>();
    }

    public Vector2 LookValue => _lookValue;
}