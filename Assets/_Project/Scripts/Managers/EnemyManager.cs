using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public event Action OnEliteSpawn;
    public event Action OnScaleUp;

    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private GameObject spawnerParent;
    
    [SerializeField]
    private Transform _normalSpawnersTransform;

    [SerializeField]
    private Transform _eliteSpawnersTransform;

    private List<EnemySpawner> _normalSpawners;
    private List<EnemySpawner> _eliteSpawners;
    private HashSet<GameObject> _enemyPrefabs;

    private Character _playerObj;
    private float _statScaling;
    private float _expScaling;

    private void Awake()
    {
        _normalSpawners = new List<EnemySpawner>();
        _eliteSpawners = new List<EnemySpawner>();
        _enemyPrefabs = new HashSet<GameObject>();
        _statScaling = 1;
        _expScaling = 1;
    }

    private void Start()
    {
        foreach (Transform child in _normalSpawnersTransform)
        {
            _normalSpawners.Add(child.GetComponent<EnemySpawner>());
            _enemyPrefabs.Add(child.GetComponent<EnemySpawner>().EnemyPrefab);
        }

        foreach (Transform child in _eliteSpawnersTransform)
            _eliteSpawners.Add(child.GetComponent<EnemySpawner>());
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

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        _playerObj.OnLevelUp += HandleOnLevelUp;
    }

    private void HandleOnLevelUp(int level, float currentExp, float nextExpToLevel)
    {
        if (level % 10 == 0)
        {
            // scale up the enemies
            _statScaling += 0.5f;
            _expScaling *= 2f;
            foreach (EnemySpawner spawner in _normalSpawners)
                spawner.ChangeScaling(_statScaling, _expScaling);

            OnScaleUp?.Invoke();
        }

        if (level % 5 == 0)
        {
            // spawn elite enemy, num depends on scaling

            for(int i = 0; i < Mathf.Floor(_statScaling + 0.5f); i++)
            {
                int randomIdx = UnityEngine.Random.Range(0, _eliteSpawners.Count);
                GameObject randomPrefab = _enemyPrefabs.ElementAt(UnityEngine.Random.Range(0, _enemyPrefabs.Count));

                _eliteSpawners[randomIdx].SpawnElite(randomPrefab, 5 * _statScaling);
            }

            OnEliteSpawn?.Invoke();
        }
    }
}
