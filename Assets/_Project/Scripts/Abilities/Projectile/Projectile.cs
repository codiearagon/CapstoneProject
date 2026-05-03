using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private IProjectileMovement _movementBehaviour;
    private IProjectileHit _hitBehaviour;

    [SerializeField] private ParticleSystem _moveParticle;
    [SerializeField] private ParticleSystem _hitParticle;

    private AudioSource _audioSource;
    private AudioClip _durationSound;
    private AudioClip _hitSound;

    private float _timeToLive;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private void FixedUpdate()
    {
        _movementBehaviour.Move(this, _rb);

        if (_moveParticle != null)
            Instantiate(_moveParticle, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hitSound != null)
            AudioSource.PlayClipAtPoint(_hitSound, transform.position);

        if (_hitParticle != null)
            Instantiate(_hitParticle, transform.position, Quaternion.identity);

        _hitBehaviour.OnHit(this, collision);
    }

    private IEnumerator StartCountdown()
    {
        if (_durationSound != null)
        {
            _audioSource.clip = _durationSound;
            _audioSource.Play();
        }

        yield return new WaitForSeconds(_timeToLive);
        Destroy(gameObject);
    }

    public void SetBehaviour(IProjectileMovement movement, IProjectileHit hit, AudioClip durationSound, AudioClip hitSound, float timeToLive, LayerMask layer)
    {
        _movementBehaviour = movement;
        _hitBehaviour = hit;
        _durationSound = durationSound;
        _hitSound = hitSound;
        _timeToLive = timeToLive;
        gameObject.layer = layer;
    }
}
