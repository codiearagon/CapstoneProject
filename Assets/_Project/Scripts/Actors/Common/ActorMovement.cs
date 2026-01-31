using UnityEngine;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(Rigidbody2D))]
public class ActorMovement : MonoBehaviour
{
    private Actor _actor;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _actor = GetComponent<Actor>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _actor.InputData.MoveValue * _actor.Stats.MovementSpeed * Time.fixedDeltaTime);
    }
}
