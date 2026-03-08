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
        IAOEMovement movement = CreateMovementBehaviour();
        IAOEHit hit = CreateHitBehaviour();

        _aoeRef = GameObject.Instantiate(_properties.AoePrefab, caster.transform.position, Quaternion.identity);
        _aoeRef.GetComponent<AOE>().SetBehaviour(movement, hit, _properties, layer);
    }

    public void Stop()
    {
        GameObject.Destroy(_aoeRef);
    }

    private IAOEMovement CreateMovementBehaviour()
    {
        switch (_properties.MovementBehaviour)
        {
            case AOEMovementBehaviour.Follow:
                return new FollowAOE(_properties);
            default:
                return null;
        }
    }

    private IAOEHit CreateHitBehaviour()
    {
        switch (_properties.HitBehaviour)
        {
            case AOEHitBehaviour.Damage:
                return new DamageAOE(_properties);
            default:
                return null;
        }
    }
}
