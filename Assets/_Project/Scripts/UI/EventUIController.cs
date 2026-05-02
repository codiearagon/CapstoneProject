using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class EventUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private EnemyManager _enemyManager;

    private Queue<(string, Color)> _eventTitleQueue;
    private Color _goodColor = new Color32(255, 215, 80, 255);
    private Color _badColor = new Color32(220, 50, 50, 255);

    private VisualElement _root;
    private Label _eventTitle;
    private VisualElement _eventDetailsContainer;
    private BossBrain _bossBrain;

    private Character _playerObj;
    private bool _titleRunning;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _eventTitleQueue = new Queue<(string, Color)>();

        _eventTitle = _root.Q<Label>("EventTitleLabel");
        _eventDetailsContainer = _root.Q<VisualElement>("EventDetailsContainer");

        _eventTitle.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
        _enemyManager.OnScaleUp += HandleScaleUp;
        _enemyManager.OnEliteSpawn += HandleEliteSpawn;
        _enemyManager.OnBossSpawned += HandleBossSpawned;
    }

    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;
        _enemyManager.OnScaleUp -= HandleScaleUp;
        _enemyManager.OnEliteSpawn -= HandleEliteSpawn;

        _playerObj.OnLevelUp -= HandleOnLevelUp;
        _playerObj.OnLevelUpBuff -= HandleOnLevelUpBuff;
        _enemyManager.OnBossSpawned -= HandleBossSpawned;
        _bossBrain.OnPhaseChange -= HandlePhaseChange;
    }

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        _playerObj.OnLevelUp += HandleOnLevelUp;
        _playerObj.OnLevelUpBuff += HandleOnLevelUpBuff;
    }

    private void HandleOnLevelUp(int level, float currentExp, float nextExpToLevel)
    {
        _eventTitleQueue.Enqueue(("Leveled up!", _goodColor));

        
        
        if(!_titleRunning)
            StartCoroutine(ShowTitle());
    }

    private void HandleOnLevelUpBuff(StatType type, float amount)
    {
        Label statBuff = new Label();
        statBuff.text = "+" + (amount * 100).ToString("F1") + "% " + type;
        statBuff.style.color = Color.white;
        statBuff.style.fontSize = 25f;

        StartCoroutine(DestroyLabel(statBuff));
        _eventDetailsContainer.Add(statBuff);
    }

    private void HandleScaleUp()
    {
        _eventTitleQueue.Enqueue(("Enemies have gotten stronger!", _badColor));

        if (!_titleRunning)
            StartCoroutine(ShowTitle());
    }

    private void HandleEliteSpawn()
    {
        _eventTitleQueue.Enqueue(("Elite enemy spawned!", _badColor));

        if (!_titleRunning)
            StartCoroutine(ShowTitle());
    }

    private void HandleBossSpawned(Enemy enemy)
    {
        _eventTitleQueue.Enqueue(("Boss spawned!", _badColor));
        _bossBrain = enemy.GetComponent<BossBrain>();

        _bossBrain.OnPhaseChange += HandlePhaseChange;

        if (!_titleRunning)
            StartCoroutine(ShowTitle());
    }

    private void HandlePhaseChange(int phase)
    {
        _eventTitleQueue.Enqueue(($"Phase {phase}!", _badColor));
        Debug.Log("Phase changed");

        if (!_titleRunning)
            StartCoroutine(ShowTitle());
    }

    private IEnumerator DestroyLabel(Label label)
    {
        yield return new WaitForSeconds(3f);
        _eventDetailsContainer.Remove(label);
    }

    private IEnumerator ShowTitle()
    {
        _titleRunning = true;

        (string text, Color color) = _eventTitleQueue.Dequeue();
        _eventTitle.text = text;
        _eventTitle.style.color = color;

        _eventTitle.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(3f);
        _eventTitle.style.display = DisplayStyle.None;

        if(_eventTitleQueue.Count > 0)
        {
            StartCoroutine(ShowTitle());
            yield break;
        }

        _titleRunning = false;
    }
}
