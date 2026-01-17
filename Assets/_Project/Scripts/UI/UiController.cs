using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    private UIDocument _uiDoc;
    private VisualElement _root;
    private Label _statsText;

    private void Awake()
    {
        _uiDoc = GetComponent<UIDocument>();
    }

    private void Start()
    {
        _root = _uiDoc.rootVisualElement;
        _statsText = _root.Q<Label>("StatsText");
    }

    private void OnEnable()
    {
        Character.OnStatsChanged += UpdateStatsUI;
    }

    private void OnDisable()
    {
        Character.OnStatsChanged -= UpdateStatsUI;
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
