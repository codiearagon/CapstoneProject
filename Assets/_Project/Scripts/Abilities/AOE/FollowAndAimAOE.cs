using UnityEngine;
using UnityEngine.TextCore.Text;

public class FollowAndAimAOE : IAOEMovement
{
    private GameObject _caster;
    private GameObject _pivot;

    public FollowAndAimAOE(AOEProperties properties, GameObject pivot)
    {
        _caster = properties.Caster;

        _pivot = pivot;
    }

    public void Move(AOE aoe)
    {
        Vector2 look = _caster.GetComponent<IActor>().GetLook();
        Vector2 pos = _caster.GetComponent<IActor>().GetPosition();

        _pivot.transform.position = pos;
        aoe.transform.SetParent(_pivot.transform);
        aoe.transform.localPosition = new Vector2(aoe.transform.localScale.x / 2, 0);

        Vector2 direction = (Camera.main.ScreenToWorldPoint(look) - _pivot.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _pivot.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}