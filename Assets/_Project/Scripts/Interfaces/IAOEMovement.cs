using UnityEngine;

public enum AOEMovementBehaviour
{
    Follow,
    FollowAndAim,
}

public interface IAOEMovement
{
    public void Move(AOE aoe);
}