using Unity.Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera _playerCam;

    public void Initialize(GameObject playerObj)
    {
        _playerCam.Follow = playerObj.transform;
    }
}
