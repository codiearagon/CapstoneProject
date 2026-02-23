using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Character : MonoBehaviour, IDamageable
{
    // Stats events
    public event Action<float> OnHealthChanged;
    public event Action<float> OnManaChanged;
    public event Action<CharacterStats> OnStatsChanged;

    // Ability events
    public event Action<List<Ability>> OnAbilitiesChanged;
    public event Action OnAbilityUnlockTriggered;
    public event Action<List<Ability>> OnAbilityUpgradeTriggered;

    // Progression events
    public event Action<float> OnExperienceReceived;
    public event Action<int, float, float> OnLevelUp;
    public event Action<List<CharacterAdvancement>> OnAdvancementTriggered;

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
    private bool _isManaRegen;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = new PlayerInput();
        _abilities = GetComponentInChildren<CharacterAbilities>();

        _stats.CurrentHp = _stats.MaxHp;
        _stats.CurrentMana = _stats.MaxMana;

        _isManaRegen = true;
        StartCoroutine(ManaRegen());

        Logger.Log("Character Initialized");
    }

    private void OnEnable()
    {
        _input.Player.Enable();
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
    }

    private void TriggerAbilityUnlock()
    {
        OnAbilityUnlockTriggered?.Invoke();

        _stats.NextAbilityUnlockLevel += 10;
    }

    private void TriggerAbilityUpgrade()
    {
        OnAbilityUpgradeTriggered?.Invoke(_abilities.GetList());

        _stats.NextAbilityUpgradeLevel += 10;
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

    public void SelectAdvancement(CharacterAdvancement advancement)
    {
        // Add bonus stats
        _stats.MaxHp += advancement.MaxHp;
        _stats.MovementSpeed += advancement.MovementSpeed;
        _stats.Attack += advancement.Attack;
        _stats.AttackSpeed += advancement.AttackSpeed;
        _stats.Defense += advancement.Defense;

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

    public void UseMana(float amount)
    {
        _stats.CurrentMana = Mathf.Clamp(_stats.CurrentMana - amount, 0, _stats.MaxMana);

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

    public CharacterStats Stats => _stats;
    public Vector2 LookValue => _lookValue;
}
