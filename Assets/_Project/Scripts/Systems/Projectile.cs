using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private IProjectileMovement _movement;
    private IProjectileHit _hit;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _timeToLive;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _damage;
    private Affinity _affinity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private void FixedUpdate()
    {
        _movement.Move(this, _rb);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _hit.OnHit(this, collision);
    }

    private IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(_timeToLive);
        _hit.OnHit(this, null);
    }

    public void SetData(float damage, Affinity affinity, Vector2 direction, LayerMask layer)
    {
        _damage = damage;
        _affinity = affinity;
        _direction = direction;
        gameObject.layer = layer;
    }

    public void SetBehaviour(IProjectileMovement movement, IProjectileHit hit)
    {
        _movement = movement;
        _hit = hit;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public float Speed => _speed;
    public float Damage => _damage;
    public Affinity Affinity => _affinity;
    public Vector2 Direction => _direction;
}
