using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Enemy : MonoBehaviour, IActor
{
    public event System.Action<float> OnDamage;
    public event System.Action OnDeath;

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

    private List<IStatusEffect> _activeStatusEffects;

    private Vector2 _lookValue;
    private bool _playerInRange;
    private bool _isPaused;
    private bool _isKnockedback;
    private float _speedMultiplier;
    private bool _isManaRegen;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _activeStatusEffects = new List<IStatusEffect>();

        _isPaused = false;
        _isKnockedback = false;

        _stats.CurrentHp = _stats.MaxHp;
        _stats.CurrentMana = _stats.MaxMana;
        _isManaRegen = true;
        StartCoroutine(ManaRegen());

        _speedMultiplier = 1;
    }

    private void Start()
    {
        _targetObj = GameObject.FindGameObjectWithTag("Character");
        _manager = FindAnyObjectByType<EnemyManager>();
        _targetRb = _targetObj.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isPaused || _isKnockedback)
            return;

        if(_targetObj != null && !_playerInRange)
        {
            _rb.MovePosition(Vector2.MoveTowards(_rb.position, _targetRb.position, (_stats.MovementSpeed / 10) * _speedMultiplier * Time.fixedDeltaTime));
            _lookValue = (_targetRb.position - _rb.position).normalized;
        }
    }

    private void Die()
    {
        _targetObj.GetComponent<Character>().ReceiveExperience(_stats.ExpOnKill);

        if(Utility.RollChance(_stats.BuffDropChance))
        {
            for(int i = 0; i < _stats.BuffDropAmount; i++)
            {
                float randomX = Random.Range(transform.position.x - 1.5f, transform.position.x + 1.5f);
                float randomY = Random.Range(transform.position.y - 1.5f, transform.position.y + 1.5f);
                Vector3 randomPos = new Vector3(randomX, randomY, 0);
                Instantiate(_buffPrefab, randomPos, Quaternion.identity);
            }
        }

        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator Knockbacked()
    {
         yield return new WaitForSeconds(1f);
        _isKnockedback = false;
        _rb.linearVelocity = Vector3.zero;
    }

    private IEnumerator ManaRegen()
    {
        while (_isManaRegen)
        {
            yield return new WaitForSeconds(1f);
            _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana + _stats.ManaRegenRate, 0, _stats.MaxMana);
        }
    }

    public bool IsDead() => _stats.CurrentHp <= 0;

    public void TakeDamage(float amount, Affinity damageAffinity)
    {
        float affinityMultiplier = AffinityLookup.GetMultiplier(damageAffinity, _stats.Affinity);
        float defenseMultiplier = 1 - (_stats.Defense / (_stats.Defense + 1000));
        float finalDamage = amount * affinityMultiplier * defenseMultiplier;
        //Logger.Log(string.Format("Received Damage: {0}, {1} base * {2}, {3}", finalDamage, amount, affinityMultiplier, _stats.EnemyName));

        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - finalDamage, 0, _stats.MaxHp);

        OnDamage?.Invoke(_stats.CurrentHp);

        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(0f, 1.5f);
        Vector3 pos = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z);
        DamageText text = Instantiate(_damageTextPrefab.transform, pos, Quaternion.identity).GetComponent<DamageText>();
        text.SetDamageText(damageAffinity, finalDamage);

        if (_stats.CurrentHp <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp + amount, 0, _stats.MaxHp);
    }

    public void FullHeal()
    {
        _stats.CurrentHp = _stats.MaxHp;
    }

    public void MultiplyStats(float multiplier, float expScaling)
    {
        _stats.MaxHp *= multiplier;
        _stats.CurrentHp *= multiplier;
        _stats.MovementSpeed = Mathf.Min(_stats.MovementSpeed * multiplier, 70);
        _stats.Attack *= multiplier;
        _stats.AttackSpeed *= multiplier;
        _stats.Defense *= multiplier;
        _stats.ExpOnKill *= expScaling;
    }

    public void MakeElite(float multiplier, float expScaling)
    {
        transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        _stats.MaxHp *= multiplier;
        _stats.CurrentHp *= multiplier;
        _stats.Attack *= multiplier;
        _stats.Defense *= multiplier;
        _stats.ExpOnKill *= expScaling;
        _stats.BuffDropChance = 100;
        _stats.BuffDropAmount = 5;
    }

    public void MakeBoss(float multiplier, float expScaling)
    {
        transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
        _stats.MaxHp *= multiplier;
        _stats.CurrentHp *= multiplier;
        _stats.Attack *= multiplier;
        _stats.Defense *= multiplier;
        _stats.ExpOnKill *= expScaling;
        _stats.BuffDropChance = 100;
        _stats.BuffDropAmount = 10;
    }

    public void PlayerInRange(bool value)
    {
        _playerInRange = value;
    }

    public bool HasMana(float amount)
    {
        return _stats.CurrentMana >= amount;
    }

    public void UseMana(float amount)
    {
        _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana - amount, 0, _stats.MaxMana);
    }

    public void GainMana(float amount)
    {
        _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana + amount, 0, _stats.MaxMana);
    }

    public void FullMana()
    {
        _stats.CurrentMana = _stats.MaxMana;
    }

    public void ApplyKnockback(Vector2 force)
    {
        _rb.AddForce(force, ForceMode2D.Impulse);
        _isKnockedback = true;
        StartCoroutine(Knockbacked());
    }

    public void ApplyMoveSpeed(float multiplier)
    {
        _speedMultiplier = multiplier;
    }

    public void ApplyStatChange(StatType stat, float multiplier)
    {
        _stats.GetStat(stat);
    }

    public void ApplyEffect(IStatusEffect effect)
    {
        IStatusEffect existing = _activeStatusEffects.FirstOrDefault(e => e.GetType() == effect.GetType());

        if(existing != null)
        {
            existing.Refresh();
            return;
        }

        Debug.Log("Status effect applied to: " + Stats.Name);
        StartCoroutine(effect.Tick(GetComponent<Collider2D>()));

        _activeStatusEffects.Add(effect);
    }

    public void RemoveEffect(IStatusEffect effect)
    {
        _activeStatusEffects.Remove(effect);
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public Vector2 GetLook()
    {
        return LookValue;
    }

    public bool IsPaused => _isPaused;
    public EnemyStats Stats => _stats;
    public EnemyManager Manager => _manager;
    public Vector2 LookValue => _lookValue;
}
