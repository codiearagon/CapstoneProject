using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAbility : IAbilityExecution
{
    private AOEProperties _properties;
    private List<IStatusEffect> _statusEffects = new List<IStatusEffect>();

    private GameObject _aoeRef;
    private GameObject _pivot;

    public AOEAbility(AOEProperties properties)
    {
        _properties = properties;
    }

    public void Execute(GameObject caster, Ability ability, LayerMask layer)
    {
        _pivot = new GameObject("BeamPivot");

        IAOEMovement movement = CreateMovementBehaviour();
        IAOEHit hit = CreateHitBehaviour();

        foreach (StatusEffectProperties statProps in _properties.StatusEffectProperties)
        {
            _statusEffects.Add(StatusEffectFactory.CreateStatusEffect(statProps));
        }

        _aoeRef = GameObject.Instantiate(_properties.AoePrefab, caster.transform.position, Quaternion.identity);
        _aoeRef.GetComponent<AOE>().SetBehaviour(movement, hit, _properties, layer);
    }

    public void Stop()
    {
        GameObject.Destroy(_aoeRef);
        GameObject.Destroy(_pivot);
    }

    private IAOEMovement CreateMovementBehaviour()
    {
        switch (_properties.MovementBehaviour)
        {
            case AOEMovementBehaviour.Follow:
                return new FollowAOE(_properties);
            case AOEMovementBehaviour.FollowAndAim:
                Logger.Log(_pivot);
                return new FollowAndAimAOE(_properties, _pivot);
            default:
                return null;
        }
    }

    private IAOEHit CreateHitBehaviour()
    {
        switch (_properties.HitBehaviour)
        {
            case AOEHitBehaviour.Damage:
                return new DamageAOE(_properties, _statusEffects);
            default:
                return null;
        }
    }
}
