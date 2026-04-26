using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IActor
{
    public event System.Action<float, float> OnDamage;
    public event System.Action OnDeath;

    [SerializeField]
    private GameObject _buffPrefab;

    [SerializeField]
    private GameObject _damageTextPrefab;

    [SerializeField]
    private EnemyStats _stats;

    private List<IStatusEffect> _activeStatusEffects;

    private EnemyMovement _enemyMovement;
    private bool _isManaRegen;
    private GameObject _target;

    private void Awake()
    {
        _activeStatusEffects = new List<IStatusEffect>();
        _enemyMovement = GetComponent<EnemyMovement>();

        _stats.CurrentHp = _stats.MaxHp;
        _stats.CurrentMana = _stats.MaxMana;
        _isManaRegen = true;
        StartCoroutine(ManaRegen());
    }

    private void Die()
    {
        _target.GetComponent<Character>().ReceiveExperience(_stats.ExpOnKill);

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

    private IEnumerator ManaRegen()
    {
        while (_isManaRegen)
        {
            yield return new WaitForSeconds(1f);
            _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana + _stats.ManaRegenRate, 0, _stats.MaxMana);
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    public bool IsDead() => _stats.CurrentHp <= 0;

    public void TakeDamage(float amount, Affinity damageAffinity)
    {
        float affinityMultiplier = AffinityLookup.GetMultiplier(damageAffinity, _stats.Affinity);
        float defenseMultiplier = 1 - (_stats.Defense / (_stats.Defense + 1000));
        float finalDamage = amount * affinityMultiplier * defenseMultiplier;

        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - finalDamage, 0, _stats.MaxHp);

        OnDamage?.Invoke(_stats.CurrentHp, _stats.MaxHp);

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

    public void HealPercent(float percentage)
    {
        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp + (_stats.MaxHp * percentage), 0, _stats.MaxHp);
    }

    public void FullHeal()
    {
        _stats.CurrentHp = _stats.MaxHp;
    }

    public void MultiplyStats(float multiplier, float expScaling, EnemyScaling scaling)
    {
        _stats.MaxHp *= 1f + (multiplier * scaling.Health);
        _stats.CurrentHp *= 1f + (multiplier * scaling.Health);
        _stats.Attack *= 1f + (multiplier * scaling.Attack);
        _stats.AttackSpeed *= 1f + (multiplier * scaling.AttackSpeed);
        _stats.MovementSpeed = Mathf.Min(_stats.MovementSpeed * (1f + (multiplier * scaling.MovementSpeed)), 70);
        _stats.Defense *= 1f + (multiplier * scaling.Defense);
        _stats.ExpOnKill *= expScaling * scaling.Experience;
    }

    public void MakeElite(float multiplier, float expScaling, EnemyScaling scaling)
    {
        transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        MultiplyStats(multiplier, expScaling, scaling);
        _stats.BuffDropChance = 100;
        _stats.BuffDropAmount = 5;
    }

    public void MakeBoss(float multiplier, float expScaling, EnemyScaling scaling)
    {
        transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
        MultiplyStats(multiplier, expScaling, scaling);
        _stats.BuffDropChance = 100;
        _stats.BuffDropAmount = 10;
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

    public void GainManaPercent(float percentage)
    {
        _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana + (_stats.MaxMana * percentage), 0, _stats.MaxMana);
    }

    public void FullMana()
    {
        _stats.CurrentMana = _stats.MaxMana;
    }

    public void ApplyKnockback(Vector2 force)
    {
        _enemyMovement.ApplyKnockback(force);
    }

    public void ApplySilence()
    {
        throw new System.NotImplementedException();
    }

    public void ApplyDisarm()
    {
        throw new System.NotImplementedException();
    }

    public void ApplyStun()
    {
        throw new System.NotImplementedException();
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

    public void AddStatModifier(StatModifier statModifier)
    {
        _stats.AddModifier(statModifier);
    }

    public void RemoveStatModifiers(object source)
    {
        _stats.RemoveModifier(source);
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public Vector2 GetLook()
    {
        return _enemyMovement.LookValue;
    }

    public EnemyStats Stats => _stats;
}
