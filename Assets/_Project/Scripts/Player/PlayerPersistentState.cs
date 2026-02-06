using UnityEngine;

public class PlayerPersistentState : MonoBehaviour
{
    public static PlayerPersistentState Instance;

    public GameObject CharacterPrefab { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void SetCharacter(GameObject charPrefab)
    {
        CharacterPrefab = charPrefab;
    }
}
