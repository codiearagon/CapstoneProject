using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _damageLabel;

    private float aliveTime = 0.5f ;

    private void Awake()
    {
        _damageLabel = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetDamageText(Affinity affinity, float damage)
    {
        _damageLabel.color = Utility.GetAffinityColor(affinity);
        _damageLabel.text = damage.ToString("N0");

        StartCoroutine(DeathTimer());
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(aliveTime);
        Destroy(gameObject);
    }
}
