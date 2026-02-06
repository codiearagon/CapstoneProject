using Unity.Cinemachine;
using UnityEngine;

public class PlayerRoot : MonoBehaviour
{
    //testing only
    [SerializeField]
    private GameObject _prefab;

    public GameObject PlayerObject { get; private set; }

    [SerializeField]
    private CinemachineCamera _playerCam;

    private void Awake()
    {
        //GameObject player = Instantiate(PlayerPersistentState.Instance.CharacterPrefab, Vector2.zero, Quaternion.identity);
        GameObject player = Instantiate(_prefab, Vector2.zero, Quaternion.identity);

        _playerCam.Follow = player.transform;
        PlayerObject = player;
    }
}
