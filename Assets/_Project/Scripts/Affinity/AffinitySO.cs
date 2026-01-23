using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Affinity", menuName = "Affinity/New Affinity")]
public class AffinitySO : ScriptableObject
{
    [Header("Details")]
    public string _name;

    [Header("Strengths & Weaknesses")]
    public List<AffinitySO> _strengths;
    public List<AffinitySO> _weaknesses;
    // add list of applyable status effects

    public float GetDmgMuliplier(AffinitySO target)
    {
        if (target == null) return 0;

        if (_strengths.Contains(target)) return 2;
        else if (_weaknesses.Contains(target)) return 0.5f;
        else return 1;
    }
}