using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _move;
    
    private Character _character;
    private Rigidbody2D _rb;
    private Stats _stats;

    private Vector2 _moveValue;

    // Gets ran by PlayerInitialize
    public void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _stats = _character.Stats;
    }

    private void OnEnable()
    {
        _move.action.Enable();
    }

    private void OnDisable()
    {
        _move.action.Disable();
    }

    private void Update()
    {
        // read input value
        _moveValue = _move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveValue * _stats.MovementSpeed * Time.fixedDeltaTime);
    }
}
