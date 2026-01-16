using UnityEngine;

[CreateAssetMenu (fileName = "New Character Base", menuName = "Character/New Character Base")]
public class CharacterBaseSO : ScriptableObject
{
    [Header("Details")]
    public string characterName;

    [Header("Base Stats")]
    public int hp;
    public int movementSpeed;
    public int attack;
    public int attackSpeed;
    public int defense;
}
