using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VisualScripting;

public class Character : MonoBehaviour, IDamageable, IManaUser
{
    // Stats events
    public event Action<float> OnHealthChanged;
    public event Action<float> OnManaChanged;
    public event Action<CharacterStats> OnStatsChanged;
    public event Action OnDeath;

    // Ability events
    public event Action<List<Ability>> OnAbilitiesChanged;
    public event Action OnAbilityUnlockTriggered;
    public event Action<List<Ability>> OnAbilityUpgradeTriggered;

    // Progression events
    public event Action<float> OnExperienceReceived;
    public event Action<int, float, float> OnLevelUp;
    public event Action<StatType, float> OnLevelUpBuff;
    public event Action<List<CharacterAdvancement>> OnAdvancementTriggered;
    public event Action<CharacterAdvancement> OnAdvancementChosen;

    [Header("Details")]
    [SerializeField]
    private CharacterStats _stats;

    [SerializeField]
    private List<CharacterAdvancement> _advancements;

    private PlayerInput _input;
    private Rigidbody2D _rb;
    private CharacterAbilities _abilities;

    private Vector2 _moveValue;
    private Vector2 _lookValue;
    private bool _isHpRegen;
    private bool _isManaRegen;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = new PlayerInput();
        _abilities = GetComponentInChildren<CharacterAbilities>();

        _stats.CurrentHp = _stats.MaxHp;
        _stats.CurrentMana = _stats.MaxMana;

        _isHpRegen = true;
        _isManaRegen = true;
        StartCoroutine(HpRegen());
        StartCoroutine(ManaRegen());

        Logger.Log("Character Initialized");
    }

    private void OnEnable()
    {
        //_input.Player.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Look.performed += OnLook;
        _input.Player.Move.canceled += OnMove;
        _input.Player.Look.canceled += OnLook;
    }

    private void OnDisable()
    {
        _input.Player.Disable();
        _input.Player.Move.performed -= OnMove;
        _input.Player.Look.performed -= OnLook;
        _input.Player.Move.canceled -= OnMove;
        _input.Player.Look.canceled -= OnLook;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _moveValue = ctx.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        _lookValue = ctx.ReadValue<Vector2>();
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

        GainRandomStats(4);
        OnLevelUp?.Invoke(_stats.Level, _stats.CurrentExp, _stats.ExpToLevelUp);

        if (_stats.Level >= _stats.NextAdvancementLevel)
            TriggerAdvancement();

        if (_stats.Level >= _stats.NextAbilityUnlockLevel)
            TriggerAbilityUnlock();

        if (_stats.Level >= _stats.NextAbilityUpgradeLevel)
            TriggerAbilityUpgrade();

        // Recursive call if exp is more than the new cap
        if (_stats.CurrentExp >= _stats.ExpToLevelUp)
            LevelUp();

        OnStatsChanged?.Invoke(_stats);
    }

    private void TriggerAdvancement()
    {
        OnAdvancementTriggered?.Invoke(_advancements);

        _stats.NextAdvancementLevel *= 2;
    }

    private void TriggerAbilityUnlock()
    {
        OnAbilityUnlockTriggered?.Invoke();

        _stats.NextAbilityUnlockLevel += 5;
    }

    private void TriggerAbilityUpgrade()
    {
        OnAbilityUpgradeTriggered?.Invoke(_abilities.GetList());

        _stats.NextAbilityUpgradeLevel += 10;
    }

    private void GainRandomStats(int amount)
    {
        for(int i = 0; i < 4; i++)
        {
            StatType stat = Utility.RollRandomStat();
            float percent = Utility.RollRandomPercentage(2, 6);
            BuffStat(stat, percent);

            OnLevelUpBuff?.Invoke(stat, percent);
        }
    }

    private IEnumerator HpRegen()
    {
        while (_isHpRegen)
        {
            yield return new WaitForSeconds(1f);
            _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp + _stats.HpRegenRate, 0, _stats.MaxHp);

            OnHealthChanged?.Invoke(_stats.CurrentHp);
        }
    }

    private IEnumerator ManaRegen()
    {
        while(_isManaRegen)
        {
            yield return new WaitForSeconds(1f);
            _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana + _stats.ManaRegenRate, 0, _stats.MaxMana);

            OnManaChanged?.Invoke(_stats.CurrentMana);
        }
    }

    // stats get scaled but move speed is capped
    public void SelectAdvancement(CharacterAdvancement advancement)
    {
        // Add bonus stats
        _stats.Affinity = advancement.Affinity;
        _stats.MaxHp += advancement.MaxHp;
        _stats.CurrentHp = _stats.MaxHp;
        _stats.HpRegenRate += advancement.HpRegenRate;
        _stats.MaxMana += advancement.MaxMana;
        _stats.CurrentMana = _stats.MaxMana;
        _stats.ManaRegenRate += advancement.ManaRegenRate;
        _stats.MovementSpeed = Mathf.Min(_stats.MovementSpeed + advancement.MovementSpeed, 120);
        _stats.Attack += advancement.Attack;
        _stats.AttackSpeed += advancement.AttackSpeed;
        _stats.Defense += advancement.Defense;
        _stats.FireMultiplier += advancement.FireMultiplier;
        _stats.WaterMultiplier += advancement.WaterMultiplier;
        _stats.AirMultiplier += advancement.AirMultiplier;
        _stats.EarthMultiplier += advancement.EarthMultiplier;
        _stats.DarkMultiplier += advancement.DarkMultiplier;
        _stats.LightMultiplier += advancement.LightMultiplier;

        // Change progression
        _stats.NextAdvancementLevel = advancement.NextAdvancementLevel;
        _advancements = advancement.Advancements;

        OnAdvancementChosen?.Invoke(advancement);
        OnStatsChanged?.Invoke(_stats);
    }

    public bool IsDead() => _stats.CurrentHp <= 0;

    public void TakeDamage(float amount, Affinity damageAffinity) 
    {
        float affinityMultiplier = AffinityLookup.GetMultiplier(damageAffinity, _stats.Affinity);
        float defenseMultiplier = 1 - (_stats.Defense / (_stats.Defense + 1000));
        float finalDamage = amount * affinityMultiplier * defenseMultiplier;
        //Logger.Log(string.Format("Received Damage: {0}, {1} base * {2}, {3}", finalDamage, amount, affinityMultiplier, _stats.CharacterName));

        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - finalDamage, 0, _stats.MaxHp);

        if(_stats.CurrentHp <= 0)
        {
            OnDeath?.Invoke();
        }

        OnHealthChanged?.Invoke(_stats.CurrentHp);
    }

    public bool HasMana(float amount)
    {
        return _stats.CurrentMana >= amount;
    }

    public void UseMana(float amount)
    {
        _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana - amount, 0, _stats.MaxMana);

        OnManaChanged?.Invoke(_stats.CurrentMana);
    }

    public void GainMana(float amount)
    {
        _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana + amount, 0, _stats.MaxMana);

        OnManaChanged?.Invoke(_stats.CurrentMana);
    }

    public void ReceiveExperience(float amount)
    {
        _stats.CurrentExp += amount;
        OnExperienceReceived?.Invoke(amount);

        if (_stats.CurrentExp >= _stats.ExpToLevelUp)
            LevelUp();
    }

    public void AddAbility(Ability ability)
    {
        _abilities.AddAbility(ability);
        OnAbilitiesChanged?.Invoke(_abilities.GetList());
    }

    public void UpgradeAbility(AbilityProperties properties)
    {
        _abilities.UpgradeAbility(properties);
        OnAbilitiesChanged?.Invoke(_abilities.GetList());
    }

    public void BuffStat(StatType type, float amount)
    {
        _stats.GetStat(type) += _stats.GetStat(type) * amount;

        _stats.MovementSpeed = Mathf.Min(_stats.MovementSpeed, 120);

        OnStatsChanged?.Invoke(_stats);
    }

    public CharacterStats Stats => _stats;
    public Vector2 LookValue => _lookValue;
}
