using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _move;

    [SerializeField]
    private InputActionReference _look;

    private Character _character;

    private ActorInputData inputData;

    public void Initialize(GameObject charObject)
    {
        inputData = new ActorInputData();
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
        inputData.MoveValue = _move.action.ReadValue<Vector2>();
        inputData.LookValue = _look.action.ReadValue<Vector2>();

        _character.UpdateInputData(inputData);
    }
}
