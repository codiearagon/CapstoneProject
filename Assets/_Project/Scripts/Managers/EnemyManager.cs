using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private GameObject spawnerParent;
    
    private List<EnemySpawner> spawners;

    private Character _playerObj;

    private void Awake()
    {
        spawners = new List<EnemySpawner>();

        foreach(Transform child in  spawnerParent.transform)
            spawners.Add(child.GetComponent<EnemySpawner>());
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
        if(level % 5 == 0)
        {
            // spawn elite enemy
        }
    }
}
