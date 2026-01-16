using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _playerActions;

    private CharacterStats _stats;
    private Rigidbody2D _rb;
    private InputAction _moveAction;

    private Vector2 _moveValue;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _stats = GetComponent<Character>().Stats;
        _moveAction = _playerActions.FindAction("Move");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
    }

    private void Update()
    {
        // read input value
        _moveValue = _moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // move player
        _rb.MovePosition(_rb.position + _moveValue * _stats.MovementSpeed * Time.fixedDeltaTime);
    }
}
