using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class AOE : MonoBehaviour
{
    private IAOEMovement _movementBehaviour;
    private IAOEHit _hitBehaviour;

    private IManaUser _manaUser;
    private float _manaCost;
    private float _attackInterval;
    private float _timeToLive;

    private AudioSource _audioSource;    
    private AudioClip _durationSound;
    private AudioClip _hitSound;

    private List<Collider2D> _enemiesInRange = new List<Collider2D>();

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        _movementBehaviour.Move(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Logger.Log("Enemies within AOE: " + _enemiesInRange.Count);
        _enemiesInRange.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _enemiesInRange.Remove(collision);
    }

    private IEnumerator OnHitCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_attackInterval);
            if (!_manaUser.HasMana(_manaCost))
                continue;

            _manaUser.UseMana(_manaCost);

            foreach (Collider2D enemy in _enemiesInRange.ToList())
            {
                if (enemy.GetComponent<ILiving>().IsDead()) continue;

                if (_hitSound != null)
                    AudioSource.PlayClipAtPoint(_hitSound, transform.position);

                _hitBehaviour.OnHit(this, enemy);
            }

            _enemiesInRange.RemoveAll(e => e == null || e.GetComponent<ILiving>().IsDead());
        }
    }

    private IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(_timeToLive);
        Destroy(gameObject);
    }

    public void SetBehaviour(IAOEMovement movement, IAOEHit hit, AOEProperties properties, LayerMask layer)
    {
        _movementBehaviour = movement;
        _hitBehaviour = hit;
        _durationSound = properties.DurationSound;
        _hitSound = properties.HitSound;
        _manaUser = properties.ManaUser;
        _manaCost = properties.ManaCost;
        _attackInterval = properties.AttackInterval;
        _timeToLive = properties.TimeToLive;
        gameObject.layer = layer;

        transform.localScale = new Vector3(properties.XSize, properties.YSize, 1f);

        if (_durationSound != null)
        {
            Debug.Log("playing aoe sound");
            _audioSource.clip = _durationSound;
            _audioSource.volume = 0.5f;
            _audioSource.Play();
        }

        // less than 0 is just infinite
        if (_timeToLive >= 0)
            StartCoroutine(StartCountdown());

        StartCoroutine(OnHitCoroutine());
    }
}
