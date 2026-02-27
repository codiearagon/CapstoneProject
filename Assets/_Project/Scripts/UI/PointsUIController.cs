using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class PointsUIController : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset _statUpgrade;

    [SerializeField]
    private PlayerRoot _playerRoot;

    private VisualElement _root;
    private Label _pointsLabel;
    private VisualElement _upgradeContainer;
    private VisualElement _statsContainer;
    private Button _applyButton;

    private Character _playerObj;

    private VisualElement _hpStat;
    private VisualElement _manaStat;
    private VisualElement _manaRegenStat;
    private VisualElement _moveSpeedStat;
    private VisualElement _attackStat;
    private VisualElement _attackSpeedStat;
    private VisualElement _defenseStat;

    private List<VisualElement> _stats;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _stats = new List<VisualElement>();

        _pointsLabel = _root.Q<Label>("PointsLabel");
        _statsContainer = _root.Q<VisualElement>("StatsContainer");
        _applyButton = _root.Q<Button>("ApplyButton");
        _upgradeContainer = _root.Q<VisualElement>("UpgradeContainer");

        _applyButton.RegisterCallback<ClickEvent>(ApplyStats);

        _upgradeContainer.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
    }
    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;
    }

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        CreateStat(_hpStat, ref _playerObj.Stats.MaxHp, "Max Hp");
        CreateStat(_manaStat, ref _playerObj.Stats.MaxMana, "Max Mana");
        CreateStat(_manaRegenStat, ref _playerObj.Stats.ManaRegenRate, "Mana Regen Rate");
        CreateStat(_moveSpeedStat, ref _playerObj.Stats.MovementSpeed, "Movement Speed");
        CreateStat(_attackStat, ref _playerObj.Stats.Attack, "Attack");
        CreateStat(_attackSpeedStat, ref _playerObj.Stats.AttackSpeed, "Attack Speed");
        CreateStat(_defenseStat, ref _playerObj.Stats.Defense, "Defense");
    }

    private void DecreaseStat(ClickEvent evt)
    {
        VisualElement stat = evt.target as VisualElement;
        StatUpgradeData data = stat.parent.parent.dataSource as StatUpgradeData;
    }

    private void IncreaseStat(ClickEvent evt)
    {
        VisualElement stat = evt.target as VisualElement;
        StatUpgradeData data = stat.parent.parent.dataSource as StatUpgradeData;
    }

    private void ApplyStats(ClickEvent evt)
    {
        foreach(VisualElement stat in _stats)
        {
            StatUpgradeData data = stat.dataSource as StatUpgradeData;
            data.OriginalStat += data.AddedStat;
        }
    }

    private void CreateStat(VisualElement stat, ref float originalStat, string name)
    {
        StatUpgradeData data = new StatUpgradeData();
        data.name = name;
        data.OriginalStat = originalStat;
        data.AddedStat = 0;

        stat = _statUpgrade.CloneTree();
        stat.dataSource = data;

        stat.Q<Button>("Increase").RegisterCallback<ClickEvent>(IncreaseStat);
        stat.Q<Button>("Decrease").RegisterCallback<ClickEvent>(DecreaseStat);
        stat.Q<Label>("StatLabel").text = System.String.Format("{0}: {1} + {2}", data.name, data.OriginalStat, data.AddedStat);

        _statsContainer.Add(stat);
        _stats.Add(stat);
    }
}
