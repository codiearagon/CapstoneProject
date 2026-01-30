// The purpose of this class is to avoid any timing issues with
// Unity's Awake and Start methods when initializing the player.
using UnityEngine;

public class PlayerInitialize : MonoBehaviour
{
    private GameObject _charPrefab;

    private void Awake()
    {
        // Get Prefab from selected character
        _charPrefab = PlayerManager.Instance.Character.prefab;
        _charPrefab.SetActive(false);

        _charPrefab.GetComponent<Character>().Initialize();
        Logger.Log("Character Initialized.");

        GetComponent<PlayerMovement>().Initialize();
        Logger.Log("Movement Initialized.");

        Logger.Log("Player is now enabled.");
    }
}
