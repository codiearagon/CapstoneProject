using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _move;

    [SerializeField]
    private InputActionReference _look;

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
        Vector2 moveValue = _move.action.ReadValue<Vector2>();
        Vector2 lookValue = _look.action.ReadValue<Vector2>();

        _character.UpdateInputData(moveValue, lookValue);
    }
}
