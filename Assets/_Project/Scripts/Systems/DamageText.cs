using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private Label _damageLabel;

    private float aliveTime = 0.5f ;

    private void Awake()
    {
        _damageLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("Label");
    }

    public void SetDamageText(Affinity affinity, float damage)
    {
        _damageLabel.style.color = Utility.GetAffinityColor(affinity);
        _damageLabel.text = damage.ToString("N0");

        StartCoroutine(DeathTimer());
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(aliveTime);
        Destroy(gameObject);
    }
}
