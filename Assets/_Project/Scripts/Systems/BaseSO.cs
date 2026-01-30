using UnityEngine;

public class BaseSO : ScriptableObject
{
    [Header("Details")]
    public string BaseName;
    public AffinitySO Affinity;

    [Header("Base Stats")]
    public int Hp;
    public int MovementSpeed;
    public int Attack;
    public int AttackSpeed;
    public int Defense;

    [Header("Objects")]
    public GameObject prefab;
}
