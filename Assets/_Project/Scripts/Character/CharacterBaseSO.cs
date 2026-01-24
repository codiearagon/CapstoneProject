using UnityEngine;

[CreateAssetMenu (fileName = "New Character Base", menuName = "Character/New Character Base")]
public class CharacterBaseSO : ScriptableObject
{
    [Header("Details")]
    public string CharacterName;
    public AffinitySO affinity;

    [Header("Sprites")]
    public Sprite icon;
    public Sprite splashArt;


    [Header("Base Stats")]
    public int Hp;
    public int MovementSpeed;
    public int Attack;
    public int AttackSpeed;
    public int Defense;
}
