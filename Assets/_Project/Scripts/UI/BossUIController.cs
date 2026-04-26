using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

public class BossUIController : MonoBehaviour
{
    [SerializeField]
    private EnemyManager _enemyManager;

    private Enemy _boss;
    private VisualElement _root;
    private Label _name;
    private ProgressBar _healthBar;


    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _name = _root.Q<Label>("Name");
        _healthBar = _root.Q<ProgressBar>("BossHealthBar");

        _root.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _enemyManager.OnBossSpawned += HandleBossSpawned;
    }

    private void OnDisable()
    {
        _enemyManager.OnBossSpawned -= HandleBossSpawned;

        if(_boss != null)
            _boss.OnDamage -= HandleDamage;
    }

    private void HandleBossSpawned(Enemy boss)
    {
        _boss = boss;

        _boss.OnDamage += HandleDamage;

        _name.text = _boss.Stats.Name;
        _healthBar.lowValue = 0;
        _healthBar.highValue = _boss.Stats.MaxHp;
        _healthBar.value = _boss.Stats.CurrentHp;

        _root.style.display = DisplayStyle.Flex;
    }

    private void HandleDamage(float amount, float maxHp)
    {
        _healthBar.value = amount;
    }
}
