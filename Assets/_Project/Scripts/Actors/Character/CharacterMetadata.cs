using UnityEngine;

public class CharacterMetadata : MonoBehaviour
{
    [SerializeField]
    public Sprite Icon;

    [SerializeField]
    public Sprite SplashArt;

    public CharacterStats Stats { get; private set; }

    private void Awake()
    {
        Stats = GetComponent<Character>().Stats;
    }
}