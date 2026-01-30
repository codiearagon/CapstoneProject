// The purpose of this class is to avoid any timing issues with
// Unity's Awake and Start methods when initializing the player.
using UnityEngine;

public class PlayerInitialize : MonoBehaviour
{
    private void Awake()
    {
        // Disable player first
        gameObject.SetActive(false);

        GetComponent<Character>().Initialize();
        Logger.Log("Character Initialized.");

        GetComponent<PlayerMovement>().Initialize();
        Logger.Log("Movement Initialized.");

        // Enable player after everything has initialized
        gameObject.SetActive(true);
        Logger.Log("Player is now enabled.");
    }
}
