using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public event System.Action OnEliteSpawn;
    public event System.Action OnScaleUp;
    public event System.Action<float> OnBossTimeLimit;
    public event System.Action<float> OnBossTimerSpawn;
    public event System.Action<Enemy> OnBossSpawned;

    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private EnemySpawner _spawner;

    [SerializeField]
    private float _bossSpawnTimer;

    [SerializeField]
    private float _bossTimeLimit;

    private Enemy _bossObject;
    private Camera _camera;
    private Character _playerObj;
    private float _statScaling;
    private float _expScaling;

    private float _currentTime;

    private void Awake()
    {
        _camera = Camera.main;
        _currentTime = _bossSpawnTimer;

        _statScaling = 0;
        _expScaling = 1;
    }

    private void Start()
    {
        _spawner.TriggerSpawning(true);
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
    }
    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;

        _playerObj.OnLevelUp -= HandleOnLevelUp;
    }

    private void OnDestroy()
    {
        if(_bossObject != null)
            _bossObject.OnDeath -= HandleOnBossDeath;
    }

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        _playerObj.OnLevelUp += HandleOnLevelUp;

        StartCoroutine(BossSpawnTimer());
        OnBossTimerSpawn?.Invoke(_currentTime);
    }

    private void HandleOnLevelUp(int level, float currentExp, float nextExpToLevel)
    {
        if (level % 5 == 0)
        {
            // scale up the enemies
            _statScaling += 0.3f;
            _expScaling *= 2f;
            _spawner.ChangeScaling(_statScaling, _expScaling);

            OnScaleUp?.Invoke();
        }

        if (level % 5 == 0)
        {
            // spawn elite enemy, num depends on scaling

            for(int i = 0; i < Mathf.Floor(_statScaling + 0.5f); i++)
            {
                _spawner.SpawnElite();
            }

            OnEliteSpawn?.Invoke();
        }
    }

    private void HandleOnBossDeath()
    {

    }

    private void SpawnBoss()
    {
        _bossObject = _spawner.SpawnBoss();
        _bossObject.OnDeath += HandleOnBossDeath;

        _spawner.TriggerSpawning(false);
        StartCoroutine(BossTimeLimit());
        OnBossSpawned?.Invoke(_bossObject);
    }

    private IEnumerator BossSpawnTimer()
    {
        while(_currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _currentTime -= 1f;
            OnBossTimerSpawn?.Invoke(_currentTime);
        }

        SpawnBoss();
    }

    private IEnumerator BossTimeLimit()
    {
        _currentTime = _bossTimeLimit;
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _currentTime -= 1f;
            OnBossTimeLimit?.Invoke(_currentTime);
        }

        _playerObj.GetComponent<ILiving>().TakeDamage(99999999999, Affinity.None);
    }
}
