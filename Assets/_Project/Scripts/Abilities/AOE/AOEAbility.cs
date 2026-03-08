using System.Collections;
using UnityEngine;

public class AOEAbility : IAbilityExecution
{
    private AOEProperties _properties;

    private GameObject _aoeRef;

    public AOEAbility(AOEProperties properties)
    {
        _properties = properties;
    }

    public void Execute(GameObject caster, Ability ability, LayerMask layer)
    {
        _aoeRef = GameObject.Instantiate(_properties.AoePrefab, caster.transform.position, Quaternion.identity);
        _aoeRef.GetComponent<AOE>().ApplyRuntimeData(caster, _properties, layer);
    }

    public void Stop()
    {
        GameObject.Destroy(_aoeRef);
    }
}
