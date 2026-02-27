using UnityEngine;
using UnityEngine.UIElements;

public class BuffDrop : MonoBehaviour
{
    private string _buffName;
    private StatType _statToBuff;
    private float _percentAmount;
    private float _pickupRange;

    private CharacterStats stats;
    private CircleCollider2D _collider;

    private Label _label;

    private void Awake()
    {
        stats = new CharacterStats();
        _collider = GetComponent<CircleCollider2D>();
        _label = GetComponent<UIDocument>().rootVisualElement.Q<Label>("Label");

        _statToBuff = Utility.RollRandomStat();
        _percentAmount = RollRandomPercentage();
        _pickupRange = 1f;

        _collider.radius = _pickupRange;

        _label.text = "+" + _percentAmount * 100 + "% " + _statToBuff;
    }

    private float RollRandomPercentage()
    {
        return Random.Range(1, 5) / 100f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Character>().BuffStat(_statToBuff, _percentAmount);
        Destroy(gameObject);
    }
}
