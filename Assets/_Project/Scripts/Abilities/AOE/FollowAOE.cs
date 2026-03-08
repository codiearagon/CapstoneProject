using UnityEngine;

public class FollowAOE : IAOEMovement
{
    private GameObject _caster;

    public FollowAOE(AOEProperties properties)
    {
        _caster = properties.Caster;
    }

    public void Move(AOE aoe)
    {
        aoe.transform.position = _caster.transform.position;
    }
}