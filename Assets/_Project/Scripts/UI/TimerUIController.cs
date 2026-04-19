using UnityEngine;
using UnityEngine.UIElements;

public class TimerUIController : MonoBehaviour
{
    [SerializeField]
    private EnemyManager _enemyManager;

    private VisualElement _root;
    private Label _timer;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _timer = _root.Q<Label>("Timer");
    }

    private void OnEnable()
    {
        _enemyManager.OnBossTimerSpawn += HandleBossTimerTick;
        _enemyManager.OnBossTimeLimit += HandleBossTimeLimit;
    }

    private void OnDisable()
    {
        _enemyManager.OnBossTimerSpawn -= HandleBossTimerTick;
        _enemyManager.OnBossTimeLimit -= HandleBossTimeLimit;
    }

    private void HandleBossTimerTick(float time)
    {
        int minute = (int)time / 60;
        int seconds = (int)time % 60;

        _timer.text = $"Boss spawns in: {minute:D2}:{seconds:D2}";
    }

    private void HandleBossTimeLimit(float time)
    {
        int minute = (int)time / 60;
        int seconds = (int)time % 60;

        _timer.text = $"Kill boss in: {minute:D2}:{seconds:D2}";
    }

}
