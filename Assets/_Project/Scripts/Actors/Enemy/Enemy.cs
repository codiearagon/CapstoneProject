using NUnit.Framework;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyStats _stats;

    private Rigidbody2D _rb;

    private GameObject _targetObj;
    private Rigidbody2D _targetRb;

    private Vector2 _lookValue;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _stats.CurrentHp = _stats.MaxHp;

        Logger.Log("Enemy Initialized");
    }

    private void Start()
    {
        _targetObj = GameObject.FindGameObjectWithTag("Character");
        _targetRb = _targetObj.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(_targetObj != null)
        {
            _rb.MovePosition(Vector2.MoveTowards(_rb.position, _targetRb.position, (_stats.MovementSpeed / 10) * Time.fixedDeltaTime));
            _lookValue = (_targetRb.position - _rb.position).normalized;
        }
    }

    private void Die()
    {
        _targetObj.GetComponent<Character>().ReceiveExperience(_stats.ExpOnKill);
        Destroy(gameObject);
    }
      

    public void TakeDamage(float amount, Affinity damageAffinity)
    {
        float affinityMultiplier = AffinityLookup.GetMultiplier(damageAffinity, _stats.Affinity);
        float finalDamage = amount * affinityMultiplier;
        Logger.Log(string.Format("Received Damage: {0}, {1} base * {2}, {3}", finalDamage, amount, affinityMultiplier, _stats.EnemyName));

        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - finalDamage, 0, _stats.MaxHp);

        if (_stats.CurrentHp <= 0)
            Die();
    }

    public EnemyStats Stats => _stats;
    public Vector2 LookValue => _lookValue;
}
