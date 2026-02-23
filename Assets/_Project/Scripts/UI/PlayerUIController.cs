using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _openStats;

    [SerializeField]
    private PlayerRoot _playerRoot;

    private Character _playerObj;

    private VisualElement _root;
    private VisualElement _mainElement;
    private VisualElement _mainTopElement;
    private VisualElement _mainBottomElement;

    private VisualElement _characterStatsElement;
    private Label _affinityLabel;
    private Label _maxHpLabel;
    private Label _maxManaLabel;
    private Label _manaRegenLabel;
    private Label _moveSpeedLabel;
    private Label _attackLabel;
    private Label _attackSpeedLabel;
    private Label _defenseLabel;
    private Label _fireMultiplierLabel;
    private Label _waterMultiplierLabel;
    private Label _airMultiplierLabel;
    private Label _earthMultiplierLabel;
    private Label _darkMultiplierLabel;
    private Label _lightMultiplierLabel;

    private ProgressBar _expBar;
    private ProgressBar _healthBar;
    private ProgressBar _manaBar;
    private Label _levelLabel;

    private VisualElement _abilityDetailsElement;
    private VisualElement _abilitiesElement;

    private Label _abilityName;
    private Label _abilityAffinity;
    private Label _abilityCost;
    private Label _abilityCooldown;
    private Label _abilityDescription;
    private Label _abilityMultiplier;

    private List<VisualElement> _abilitySlots;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _mainElement = _root.Q<VisualElement>("MainUI");
        _mainTopElement = _mainElement.Q<VisualElement>("MainTopElement");
        _mainBottomElement = _mainElement.Q<VisualElement>("MainBottomElement");

        _healthBar = _mainTopElement.Q<ProgressBar>("CharHealthBar");
        _manaBar = _mainTopElement.Q<ProgressBar>("CharManaBar");
        _levelLabel = _mainTopElement.Q<Label>("LevelLabel");
        _expBar = _mainBottomElement.Q<ProgressBar>("ExpBar");

        _characterStatsElement = _root.Q<VisualElement>("CharacterStats");
        _affinityLabel = _characterStatsElement.Q<Label>("AffinityLabel");
        _maxHpLabel = _characterStatsElement.Q<Label>("MaxHpLabel");
        _maxManaLabel = _characterStatsElement.Q<Label>("MaxManaLabel");
        _manaRegenLabel = _characterStatsElement.Q<Label>("ManaRegenLabel");
        _moveSpeedLabel = _characterStatsElement.Q<Label>("MoveSpeedLabel");
        _attackLabel = _characterStatsElement.Q<Label>("AttackLabel");
        _attackSpeedLabel = _characterStatsElement.Q<Label>("AttackSpeedLabel");
        _defenseLabel = _characterStatsElement.Q<Label>("DefenseLabel");
        _fireMultiplierLabel = _characterStatsElement.Q<Label>("FireMultiplierLabel");
        _waterMultiplierLabel = _characterStatsElement.Q<Label>("WaterMultiplierLabel");
        _airMultiplierLabel = _characterStatsElement.Q<Label>("AirMultiplierLabel");
        _earthMultiplierLabel = _characterStatsElement.Q<Label>("EarthMultiplierLabel");
        _darkMultiplierLabel = _characterStatsElement.Q<Label>("DarkMultiplierLabel");
        _lightMultiplierLabel = _characterStatsElement.Q<Label>("LightMultiplierLabel");

        _abilityDetailsElement = _mainBottomElement.Q<VisualElement>("AbilityDetailsElement");
        _abilitiesElement = _mainBottomElement.Q<VisualElement>("AbilitiesElement");

        _abilitySlots = _abilitiesElement.Query("AbilitySlot").ToList();
        _abilityName = _abilityDetailsElement.Q<Label>("Name");
        _abilityAffinity = _abilityDetailsElement.Q<Label>("Affinity");
        _abilityCost = _abilityDetailsElement.Q<Label>("ManaCost");
        _abilityCooldown = _abilityDetailsElement.Q<Label>("Cooldown");
        _abilityDescription = _abilityDetailsElement.Q<Label>("Description");
        _abilityMultiplier = _abilityDetailsElement.Q<Label>("AttackMultiplier");

        _characterStatsElement.style.display = DisplayStyle.None;
        _abilityDetailsElement.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _openStats.action.Enable();
        _openStats.action.performed += OnOpenStats;

        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void OnDisable()
    {
        _openStats.action.Disable();
        _openStats.action.performed -= OnOpenStats;

        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;
        _playerObj.OnStatsChanged -= UpdateStatsUI;
        _playerObj.OnExperienceReceived -= AddExperience;
        _playerObj.OnLevelUp -= HandleLevelUp;
        _playerObj.OnHealthChanged -= HandleTakeDamage;
        _playerObj.OnManaChanged -= HandleManaChanged;
        _playerObj.OnAbilitiesChanged -= HandleAbilityChanged;

        foreach (VisualElement slot in _abilitySlots)
        {
            slot.UnregisterCallback<MouseEnterEvent>(HandleMouseEnter);
            slot.UnregisterCallback<MouseLeaveEvent>(HandleMouseLeave);
        }
    }

    private void HandlePlayerSpawned(GameObject playerObj)
    {
        _playerObj = playerObj.GetComponent<Character>();
        _playerObj.OnStatsChanged += UpdateStatsUI;
        _playerObj.OnExperienceReceived += AddExperience;
        _playerObj.OnLevelUp += HandleLevelUp;
        _playerObj.OnHealthChanged += HandleTakeDamage;
        _playerObj.OnManaChanged += HandleManaChanged;
        _playerObj.OnAbilitiesChanged += HandleAbilityChanged;

        foreach (VisualElement slot in _abilitySlots)
        {
            slot.RegisterCallback<MouseEnterEvent>(HandleMouseEnter);
            slot.RegisterCallback<MouseLeaveEvent>(HandleMouseLeave);
        }

        UpdateStatsUI(_playerObj.Stats);
    }

    private void UpdateStatsUI(CharacterStats stats)
    {
        _levelLabel.text = "Level " + stats.Level;
        _healthBar.lowValue = 0;
        _healthBar.highValue = stats.MaxHp;
        _healthBar.value = stats.CurrentHp;

        _affinityLabel.text = "Affinity: " + stats.Affinity.ToString();
        _maxHpLabel.text = "Max HP: " + stats.MaxHp;
        _maxManaLabel.text = "Max Mana: " + stats.MaxMana;
        _manaRegenLabel.text = "Mana Regen: " + stats.ManaRegenRate;
        _moveSpeedLabel.text = "Movement Speed: " + stats.MovementSpeed;
        _attackLabel.text = "Attack: " + stats.Attack;
        _attackSpeedLabel.text = "Attack Speed: " + stats.AttackSpeed;
        _defenseLabel.text = "Defense: " + stats.Defense;
        _fireMultiplierLabel.text = "Fire Multiplier: " + stats.FireMultiplier * 100 + "%";
        _waterMultiplierLabel.text = "Water Multiplier: " + stats.WaterMultiplier * 100 + "%";
        _airMultiplierLabel.text = "Air Multiplier: " + stats.AirMultiplier * 100 + "%";
        _earthMultiplierLabel.text = "Earth Multiplier: " + stats.EarthMultiplier * 100 + "%";
        _darkMultiplierLabel.text = "Dark Multiplier: " + stats.DarkMultiplier * 100 + "%";
        _lightMultiplierLabel.text = "Light Multiplier: " + stats.LightMultiplier * 100 + "%";
    }

    private void AddExperience(float amount)
    {
        _expBar.value += amount;
    }

    private void HandleLevelUp(int level, float currentExp, float nextExpToLevel)
    {
        _levelLabel.text = "Level " + _playerObj.Stats.Level;
        _expBar.highValue = nextExpToLevel;
        _expBar.value = currentExp;
    }

    private void HandleTakeDamage(float amount)
    {
        _healthBar.value = amount;
    }

    private void HandleManaChanged(float amount)
    {
        _manaBar.value = amount;
    }

    private void OnOpenStats(InputAction.CallbackContext context)
    {
        if (_characterStatsElement.style.display == DisplayStyle.None)
            _characterStatsElement.style.display = DisplayStyle.Flex;
        else
            _characterStatsElement.style.display = DisplayStyle.None;

    }

    private void HandleMouseEnter(MouseEnterEvent evt)
    {
        VisualElement slot = evt.target as VisualElement;

        if (slot.dataSource == null)
            return;

        Ability ability = slot.dataSource as Ability;

        _abilityName.text = ability.Properties  .AbilityName;
        _abilityAffinity.text = ability.Properties.Affinity.ToString();
        _abilityCost.text = ability.Properties.ManaCost.ToString() + " mana";
        _abilityCooldown.text = ability.Properties.CooldownTime.ToString() + " secs";
        _abilityDescription.text = ability.Properties.Description;
        _abilityMultiplier.text = ability.Properties.AttackMultiplier * 100 + "% of attack";

        _abilityDetailsElement.style.display = DisplayStyle.Flex;

    }

    private void HandleMouseLeave(MouseLeaveEvent evt)
    {
        VisualElement slot = evt.target as VisualElement;

        if (slot.dataSource == null)
            return;

        _abilityDetailsElement.style.display = DisplayStyle.None;
    }

    private void HandleAbilityChanged(List<Ability> abilities)
    {
        for(int i = 0; i < abilities.Count; i++)
        {
            _abilitySlots[i].dataSource = abilities[i];

            _abilitySlots[i].Q<Image>("Image").image = abilities[i].Properties.Icon.texture;
            _abilitySlots[i].Q<Label>("Cooldown").text = abilities[i].CooldownRemaining.ToString("0.#");
        }
    }
}