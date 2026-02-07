using UnityEngine;

public class Projectile : MonoBehaviour
{
    private IProjectileMovement _movement;
    private IProjectileHit _hit;

    [SerializeField]
    private float _speed;

    private float _timeToLive;

    private Rigidbody2D _rb;
    private float _damage;
    private Affinity _affinity;
    private Vector2 _direction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetProperties(float damage, Vector2 direction, Affinity affinity, LayerMask layer)
    {
        _damage = damage;
        _direction = direction;
        _affinity = affinity;
        gameObject.layer = layer;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _direction * _speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<IDamageable>()?.TakeDamage(_damage, _affinity);
        Destroy(gameObject);
    }
}
