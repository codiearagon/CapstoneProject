using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPersistentState : MonoBehaviour
{
    public static PlayerPersistentState Instance;

    public Character Character { get; private set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

    public void SetCharacter(Character character)
    {
        Character = character;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "Main":
                Instantiate(Character, Vector2.zero, Quaternion.identity);
                break;
            default:
                break;
        }
    }
}
