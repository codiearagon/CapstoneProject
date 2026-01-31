using UnityEngine;

public class PlayerPersistentState : MonoBehaviour
{
    public static PlayerPersistentState Instance;

    public CharacterBaseSO Character { get; private set; }

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

    public void SetCharacter(CharacterBaseSO character)
    {
        Character = character;
    }
}
