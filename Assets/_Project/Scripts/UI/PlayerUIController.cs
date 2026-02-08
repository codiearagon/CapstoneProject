using System;
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
    private Label _maxHpLabel;
    private Label _moveSpeedLabel;
    private Label _attackLabel;
    private Label _attackSpeedLabel;
    private Label _defenseLabel;

    private ProgressBar _expBar;
    private ProgressBar _healthBar;
    private Label _levelLabel;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _mainElement = _root.Q<VisualElement>("MainUI");
        _mainTopElement = _mainElement.Q<VisualElement>("MainTopElement");
        _mainBottomElement = _mainElement.Q<VisualElement>("MainBottomElement");

        _healthBar = _mainTopElement.Q<ProgressBar>("CharHealthBar");
        _levelLabel = _mainTopElement.Q<Label>("LevelLabel");
        _expBar = _mainBottomElement.Q<ProgressBar>("ExpBar");

        _characterStatsElement = _root.Q<VisualElement>("CharacterStats");
        _maxHpLabel = _characterStatsElement.Q<Label>("MaxHpLabel");
        _moveSpeedLabel = _characterStatsElement.Q<Label>("MoveSpeedLabel");
        _attackLabel = _characterStatsElement.Q<Label>("AttackLabel");
        _attackSpeedLabel = _characterStatsElement.Q<Label>("AttackSpeedLabel");
        _defenseLabel = _characterStatsElement.Q<Label>("DefenseLabel");

        _characterStatsElement.style.display = DisplayStyle.None;
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
    }

    private void HandlePlayerSpawned(GameObject playerObj)
    {
        _playerObj = playerObj.GetComponent<Character>();
        _playerObj.OnStatsChanged += UpdateStatsUI;
        _playerObj.OnExperienceReceived += AddExperience;
        _playerObj.OnLevelUp += HandleLevelUp;
        _playerObj.OnHealthChanged += HandleTakeDamage;

        UpdateStatsUI(_playerObj.Stats);
    }

    private void UpdateStatsUI(CharacterStats stats)
    {
        _levelLabel.text = "Level " + stats.Level;
        _healthBar.lowValue = 0;
        _healthBar.highValue = stats.MaxHp;
        _healthBar.value = stats.CurrentHp;

        _maxHpLabel.text = "Max HP: " + stats.MaxHp;
        _moveSpeedLabel.text = "Movement Speed: " + stats.MovementSpeed;
        _attackLabel.text = "Attack: " + stats.Attack;
        _attackSpeedLabel.text = "Attack Speed: " + stats.AttackSpeed;
        _defenseLabel.text = "Defense: " + stats.Defense;
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

    private void OnOpenStats(InputAction.CallbackContext context)
    {
        if (_characterStatsElement.style.display == DisplayStyle.None)
            _characterStatsElement.style.display = DisplayStyle.Flex;
        else
            _characterStatsElement.style.display = DisplayStyle.None;

    }
}