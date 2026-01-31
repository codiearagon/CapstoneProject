using UnityEngine;

public class ActorBaseSO : ScriptableObject
{
    [Header("Instantiated Prefab")]
    public GameObject prefab;

    [Header("Details")]
    public string ActorName;
    public AffinitySO Affinity;

    [Header("Base Stats")]
    public int Hp;
    public int MovementSpeed;
    public int Attack;
    public int AttackSpeed;
    public int Defense;
}
