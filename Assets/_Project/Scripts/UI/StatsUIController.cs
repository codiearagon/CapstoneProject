using UnityEngine;
using UnityEngine.UIElements;

public class StatsUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerRoot _playerRoot;

    private Character _playerObj;

    private VisualElement _root;
    private Label _statsText;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _statsText = _root.Q<Label>("StatsText");
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
        _playerObj.OnStatsChanged += UpdateStatsUI;
    }

    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;
        _playerObj.OnStatsChanged -= UpdateStatsUI;
    }

    private void HandlePlayerSpawned(GameObject playerObj)
    {
        _playerObj = playerObj.GetComponent<Character>();
        UpdateStatsUI(_playerObj.Stats);
    }

    private void UpdateStatsUI(CharacterStats stats)
    {
        _statsText.text = System.String.Format("Stats:\n" +
                                               "Max HP: {0}\n" +
                                               "Current HP: {1}\n" +
                                               "Movement Speed: {2}\n" +
                                               "Attack: {3}\n" +
                                               "Attack Speed: {4}\n" +
                                               "Defense: {5}", stats.MaxHp, stats.CurrentHp, stats.MovementSpeed, stats.Attack, stats.AttackSpeed, stats.Defense);
    }
}
