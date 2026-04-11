using TMPro;
using UnityEngine;

public class BuffDrop : MonoBehaviour
{
    private string _buffName;
    private StatType _statToBuff;
    private float _percentAmount;
    private float _pickupRange;

    private CharacterStats stats;
    private CircleCollider2D _collider;

    private TextMeshProUGUI _label;

    private void Awake()
    {
        stats = new CharacterStats();
        _collider = GetComponent<CircleCollider2D>();
        _label = GetComponentInChildren<TextMeshProUGUI>();

        _statToBuff = Utility.RollRandomStat();
        _percentAmount = Utility.RollRandomPercentage(1, 3);
        _pickupRange = 1f;

        _collider.radius = _pickupRange;

        _label.text = "+" + (_percentAmount * 100).ToString("F1") + "% " + _statToBuff;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Character>().BuffStat(_statToBuff, _percentAmount);
        Destroy(gameObject);
    }
}
