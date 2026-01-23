using UnityEngine;

[CreateAssetMenu (fileName = "New Character Base", menuName = "Character/New Character Base")]
public class CharacterBaseSO : ScriptableObject
{
    [Header("Details")]
    public string CharacterName;
    public Sprite splashArt;
    public AffinitySO affinity;

    [Header("Base Stats")]
    public int Hp;
    public int MovementSpeed;
    public int Attack;
    public int AttackSpeed;
    public int Defense;
}
