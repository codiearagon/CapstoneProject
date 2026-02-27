using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EventUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerRoot _playerRoot;

    private VisualElement _root;
    private Label _eventTitle;
    private VisualElement _eventDetailsContainer;

    private Character _playerObj;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _eventTitle = _root.Q<Label>("EventTitleLabel");
        _eventDetailsContainer = _root.Q<VisualElement>("EventDetailsContainer");

        _eventTitle.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
    }
    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;

        _playerObj.OnLevelUp -= HandleOnLevelUp;
        _playerObj.OnLevelUpBuff -= HandleOnLevelUpBuff;
    }

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        _playerObj.OnLevelUp += HandleOnLevelUp;
        _playerObj.OnLevelUpBuff += HandleOnLevelUpBuff;
    }

    private void HandleOnLevelUp(int level, float currentExp, float nextExpToLevel)
    {
        _eventTitle.text = "Leveled up!";

        _eventTitle.style.display = DisplayStyle.Flex;
        StartCoroutine(HideTitle());
    }

    private void HandleOnLevelUpBuff(StatType type, float amount)
    {
        Label statBuff = new Label();
        statBuff.text = "+" + amount * 100 + "% " + type;
        statBuff.style.color = Color.white;
        statBuff.style.fontSize = 25f;

        StartCoroutine(DestroyLabel(statBuff));
        _eventDetailsContainer.Add(statBuff);
    }

    private IEnumerator DestroyLabel(Label label)
    {
        yield return new WaitForSeconds(3f);
        _eventDetailsContainer.Remove(label);
    }

    private IEnumerator HideTitle()
    {
        yield return new WaitForSeconds(4f);
        _eventTitle.style.display = DisplayStyle.None;
    }
}
