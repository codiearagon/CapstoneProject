using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _move;

    [SerializeField]
    private InputActionReference _look;
  
    private Vector2 _moveValue;
    private Vector2 _lookValue;

    private Character _character;

    public void Initialize(GameObject charObject)
    {
        _character = charObject.GetComponent<Character>();
    }

    private void OnEnable()
    {
        _move.action.Enable();
        _look.action.Enable();
    }

    private void OnDisable()
    {
        _move.action.Disable();
        _look.action.Disable();
    }

    private void Update()
    {
        // read input value
        _moveValue = _move.action.ReadValue<Vector2>();
        _lookValue = _look.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _character.Move(_moveValue);
        _character.Look(_lookValue);
    }
}
