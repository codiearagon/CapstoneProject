using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public event System.Action OnEliteSpawn;
    public event System.Action OnScaleUp;

    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private EnemySpawner _spawner;

    private Camera _camera;
    private Character _playerObj;
    private float _statScaling;
    private float _expScaling;

    private void Awake()
    {
        _camera = Camera.main;

        _statScaling = 1;
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

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        _playerObj.OnLevelUp += HandleOnLevelUp;
    }

    private void HandleOnLevelUp(int level, float currentExp, float nextExpToLevel)
    {
        if (level % 5 == 0)
        {
            // scale up the enemies
            _statScaling += 0.5f;
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
}
