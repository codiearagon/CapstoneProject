using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private TMP_Text _statsText;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }

    private void OnEnable()
    {
        Character.OnStatsChanged += UpdateUI;
    }

    private void OnDisable()
    {
        Character.OnStatsChanged -= UpdateUI;
    }

    public void UpdateUI(CharacterStats stats)
    {
        _statsText.text = System.String.Format("Stats:\n" +
                                               "Max Hp: {0}\n" +
                                               "Movement Speed: {1}\n" +
                                               "Attack: {2}\n" +
                                               "Attack Speed: {3}\n" +
                                               "Defense: {4}", stats.MaxHp, stats.MovementSpeed, stats.Attack, stats.AttackSpeed, stats.Defense);
    }
}
