using System;
using UnityEngine;

public class PlayerInitialize : MonoBehaviour
{
    private GameObject _charPrefab;
    private GameObject _instancedCharacter;

    private void Awake()
    {
        gameObject.SetActive(false);

        if(PlayerPersistentState.Instance == null || PlayerPersistentState.Instance.Character == null)
        {
            Logger.Log("No character selected.");
            return;
        }

        Initialize(PlayerPersistentState.Instance.Character);
    }

    public void Initialize(CharacterBaseSO baseData)
    {
        Logger.Log("----Player Initialization started----");
        
        // Get Prefab from selected character
        _charPrefab = baseData.prefab;

        // Instantiate from prefab
        _instancedCharacter = Instantiate(_charPrefab, Vector2.zero, Quaternion.identity);

        Logger.Log("Initializing character: " + baseData.ActorName);
        _instancedCharacter.GetComponent<Character>().InitializeActor(baseData);
        Logger.Log("Character Initialized.");

        GetComponent<PlayerInput>().Initialize(_instancedCharacter);
        Logger.Log("Player Input Initialized.");

        GetComponent<PlayerCamera>().Initialize(_instancedCharacter);
        Logger.Log("Player Camera Initialized.");

        gameObject.SetActive(true);
        Logger.Log("Player is now enabled.");

        Logger.Log("----Player Initialization finished----");
    }
}
