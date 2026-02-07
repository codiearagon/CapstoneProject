using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour, IDamageable
{
    public event Action<float> OnHealthChanged;
    public event Action<CharacterStats> OnStatsChanged;
    public event Action<List<CharacterAdvancement>> OnAdvancementTriggered;
    public event Action<float> OnExperienceReceived;
    public event Action<int, float, float> OnLevelUp;

    [Header("References")]
    [SerializeField]
    private InputActionReference _moveRef;

    [SerializeField]
    private InputActionReference _lookRef;

    [Header("Details")]
    [SerializeField]
    private CharacterStats _stats;

    [SerializeField]
    private List<CharacterAdvancement> _advancements;

    private Rigidbody2D _rb;

    private Vector2 _moveValue;
    private Vector2 _lookValue;

    private void OnEnable()
    {
        _moveRef.action.Enable();
        _lookRef.action.Enable();
    }

    private void OnDisable()
    {
        _moveRef.action.Disable();
        _lookRef.action.Disable();
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _stats.CurrentHp = _stats.MaxHp;

        Logger.Log("Character Initialized");
    }

    private void Update()
    {
        _lookValue = _lookRef.action.ReadValue<Vector2>();
        _moveValue = _moveRef.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveValue * (_stats.MovementSpeed / 10) * Time.deltaTime);
    }

    private void LevelUp()
    {
        _stats.Level++;
        _stats.CurrentExp -= _stats.ExpToLevelUp;
        _stats.ExpToLevelUp = _stats.ExpToLevelUp * 1.2f;

        OnLevelUp?.Invoke(_stats.Level, _stats.CurrentExp, _stats.ExpToLevelUp);


        if (_stats.Level >= _stats.NextAdvancementLevel)
            TriggerAdvancement();

        // Recursive call if exp is more than the new cap
        if (_stats.CurrentExp >= _stats.ExpToLevelUp)
            LevelUp();

        OnStatsChanged?.Invoke(_stats);
    }

    private void TriggerAdvancement()
    {
        OnAdvancementTriggered?.Invoke(_advancements);
    }

    public void SelectAdvancement(CharacterAdvancement advancement)
    {
        // Add bonus stats
        _stats.MaxHp += advancement.MaxHp;
        _stats.MovementSpeed += advancement.MovementSpeed;
        _stats.Attack += advancement.Attack;
        _stats.AttackSpeed += advancement.AttackSpeed;
        _stats.Defense += advancement.Defense;

        // Add new scalings
        _stats.HpScaling += advancement.HpScaling;
        _stats.AttackScaling += advancement.AttackScaling;
        _stats.DefenseScaling += advancement.DefenseScaling;

        // Change progression
        _stats.NextAdvancementLevel = advancement.NextAdvancementLevel;
        _advancements = advancement.Advancements;
    }

    public void TakeDamage(float amount, Affinity damageAffinity)
    {
        float affinityMultiplier = AffinityLookup.GetMultiplier(damageAffinity, _stats.Affinity);
        float finalDamage = amount * affinityMultiplier;
        //Logger.Log(string.Format("Received Damage: {0}, {1} base * {2}, {3}", finalDamage, amount, affinityMultiplier, _stats.CharacterName));

        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - finalDamage, 0, _stats.MaxHp);

        Color objCol = GetComponent<SpriteRenderer>().color;
        objCol.a = (_stats.CurrentHp) / (_stats.MaxHp);

        GetComponent<SpriteRenderer>().color = objCol;

        OnHealthChanged?.Invoke(_stats.CurrentHp);
    }

    public void ReceiveExperience(float amount)
    {
        _stats.CurrentExp += amount;
        OnExperienceReceived?.Invoke(amount);

        if (_stats.CurrentExp >= _stats.ExpToLevelUp)
            LevelUp();
    }

    public CharacterStats Stats => _stats;
    public Vector2 LookValue => _lookValue;
}
