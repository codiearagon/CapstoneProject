using System;
using UnityEngine;

public enum GameState
{
    MENU,
    IN_GAME,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private DeathUIController _deathUI;

    private Character _playerObj;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
    }

    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;
        _playerObj.OnDeath -= HandleOnDeath;
    }

    private void HandlePlayerSpawned(GameObject playerObj)
    {
        _playerObj = playerObj.GetComponent<Character>();
        _playerObj.OnDeath += HandleOnDeath;
    }

    private void HandleOnDeath()
    {
        Utility.RequestPause();
        _deathUI.TriggerUI();
        Logger.Log("Player died");
    }
}
