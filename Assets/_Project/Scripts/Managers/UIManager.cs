using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private TMP_Text _statsText;

    [SerializeField]
    private CharacterStats _stats;

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

    public void UpdateUI()
    {
        _statsText.text = System.String.Format("Stats:\n" +
                                               "Max Hp: {0}\n" +
                                               "Movement Speed: {1}\n" +
                                               "Attack: {2}\n" +
                                               "Attack Speed: {3}\n" +
                                               "Defense: {4}", _stats.MaxHp, _stats.MovementSpeed, _stats.Attack, _stats.AttackSpeed, _stats.Defense);
    }
}
