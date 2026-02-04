using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Base", menuName = "Base/New Enemy Base")]
public class EnemyBaseSO : ScriptableObject
{
    [Header("Instantiated Prefab")]
    public GameObject Prefab;

    [Header("Details")]
    public string ActorName;

    [Header("Base Stats")]
    public int Hp;
    public int MovementSpeed;
    public int Attack;
    public float AttackSpeed;
    public int Defense;
    public float AggroRadius;
    public float AttackRange;
}
