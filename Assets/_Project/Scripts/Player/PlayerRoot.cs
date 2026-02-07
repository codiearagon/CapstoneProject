using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    public event Action<GameObject> OnPlayerSpawned;

    //testing only
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private CinemachineCamera _playerCam;

    private GameObject _playerObj;

    private void Awake()
    {
        //GameObject player = Instantiate(PlayerPersistentState.Instance.CharacterPrefab, Vector2.zero, Quaternion.identity);
        GameObject player = Instantiate(_prefab, Vector2.zero, Quaternion.identity);

        _playerCam.Follow = player.transform;
        _playerObj = player;
    }

    private void Start()
    {
        OnPlayerSpawned?.Invoke(_playerObj);
    }
}
