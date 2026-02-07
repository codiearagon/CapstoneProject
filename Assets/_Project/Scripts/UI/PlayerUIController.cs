using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerRoot _playerRoot;

    private Character _playerObj;

    private VisualElement _root;
    private VisualElement _mainTopElement;
    private VisualElement _mainBottomElement;
    private ProgressBar _expBar;
    private ProgressBar _healthBar;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _mainTopElement = _root.Q<VisualElement>("MainTopElement");
        _mainBottomElement = _root.Q<VisualElement>("MainBottomElement");

        _healthBar = _mainTopElement.Q<ProgressBar>("CharHealthBar");
        _expBar = _mainBottomElement.Q<ProgressBar>("ExpBar");
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void OnDisable()
    {
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
        _healthBar.lowValue = 0;
        _healthBar.highValue = stats.MaxHp;
        _healthBar.value = stats.CurrentHp;
    }

    private void AddExperience(float amount)
    {
        _expBar.value += amount;
    }

    private void HandleLevelUp(int level, float currentExp, float currentExpToLevel, float nextExpToLevel)
    {
        _expBar.lowValue = currentExpToLevel;
        _expBar.highValue = nextExpToLevel;
        _expBar.value = currentExp;
    }

    private void HandleTakeDamage(float amount)
    {
        _healthBar.value = amount;
    }
}