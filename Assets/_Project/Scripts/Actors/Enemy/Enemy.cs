using NUnit.Framework;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private GameObject _buffPrefab;

    [SerializeField]
    private GameObject _damageTextPrefab;

    [SerializeField]
    private EnemyStats _stats;

    private Rigidbody2D _rb;

    private EnemyManager _manager;
    private GameObject _targetObj;
    private Rigidbody2D _targetRb;

    private Vector2 _lookValue;
    private bool _playerInRange;
    private bool _isPaused;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _isPaused = false;
        _stats.CurrentHp = _stats.MaxHp;
    }

    private void Start()
    {
        _targetObj = GameObject.FindGameObjectWithTag("Character");
        _manager = FindAnyObjectByType<EnemyManager>();
        _targetRb = _targetObj.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isPaused)
            return;

        if(_targetObj != null && !_playerInRange)
        {
            _rb.MovePosition(Vector2.MoveTowards(_rb.position, _targetRb.position, (_stats.MovementSpeed / 10) * Time.fixedDeltaTime));
            _lookValue = (_targetRb.position - _rb.position).normalized;
        }
    }

    private void Die()
    {
        _targetObj.GetComponent<Character>().ReceiveExperience(_stats.ExpOnKill);

        if(Utility.RollChance(50))
            Instantiate(_buffPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void HandlePause()
    {
        _isPaused = !_isPaused;
    }
      

    public void TakeDamage(float amount, Affinity damageAffinity)
    {
        float affinityMultiplier = AffinityLookup.GetMultiplier(damageAffinity, _stats.Affinity);
        float defenseMultiplier = 1 - (_stats.Defense / (_stats.Defense + 1000));
        float finalDamage = amount * affinityMultiplier * defenseMultiplier;
        //Logger.Log(string.Format("Received Damage: {0}, {1} base * {2}, {3}", finalDamage, amount, affinityMultiplier, _stats.EnemyName));

        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - finalDamage, 0, _stats.MaxHp);

        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(0f, 1.5f);
        Vector3 pos = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z);
        DamageText text = Instantiate(_damageTextPrefab.transform, pos, Quaternion.identity).GetComponent<DamageText>();
        text.SetDamageText(damageAffinity, finalDamage);

        if (_stats.CurrentHp <= 0)
            Die();
    }

    public void PlayerInRange(bool value)
    {
        _playerInRange = value;
    }

    public bool IsPaused => _isPaused;
    public EnemyStats Stats => _stats;
    public EnemyManager Manager => _manager;
    public Vector2 LookValue => _lookValue;
}
