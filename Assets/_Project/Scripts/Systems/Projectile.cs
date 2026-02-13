using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private IProjectileMovement _movement;
    private IProjectileHit _hit;

    private float _timeToLive;
    private Rigidbody2D _rb;

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

    public void SetBehaviour(IProjectileMovement movement, IProjectileHit hit, float timeToLive, LayerMask layer)
    {
        _movement = movement;
        _hit = hit;
        _timeToLive = timeToLive;
        gameObject.layer = layer;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
