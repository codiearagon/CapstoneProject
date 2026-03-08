using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class AOE : MonoBehaviour
{
    private AOEProperties _properties;
    private GameObject _followTarget;

    private List<IDamageable> _enemiesInRange = new List<IDamageable>();

    private void Start()
    {
        // less than 0 is just infinite
        if (_properties.TimeToLive >= 0)
            StartCoroutine(StartCountdown());

        StartCoroutine(DamageCoroutine());
    }

    private void FixedUpdate()
    {
        transform.position = _followTarget.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _enemiesInRange.Add(collision?.GetComponent<IDamageable>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _enemiesInRange.Remove(collision?.GetComponent<IDamageable>());
    }

    private IEnumerator DamageCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_properties.AttackInterval);

            foreach (IDamageable enemy in _enemiesInRange.ToList())
            {
                if (enemy.IsDead()) continue;

                enemy.TakeDamage(_properties.Damage, _properties.Affinity);
            }

            _enemiesInRange.RemoveAll(e => e == null || e.IsDead());
        }
    }

    private IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(_properties.TimeToLive);
        Destroy(gameObject);
    }

    public void ApplyRuntimeData(GameObject caster, AOEProperties properties, LayerMask layer)
    {
        _followTarget = caster;
        _properties = properties;
        gameObject.layer = layer;

        transform.localScale = new Vector3(_properties.Size, _properties.Size, 0);
    }
}
