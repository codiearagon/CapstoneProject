using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Base", menuName = "Base/New Enemy Base")]
public class EnemyBaseSO : ActorBaseSO
{
    [Header("Properties")]
    public float AggroRadius;
    public float AttackRange;
}
