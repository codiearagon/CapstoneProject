using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset playerActions;

    private CharacterStats stats;
    private Rigidbody2D rb;
    private InputAction moveAction;

    private Vector2 moveValue;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        moveAction = playerActions.FindAction("Move");
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void Update()
    {
        // read input value
        moveValue = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // move player
        rb.MovePosition(rb.position + moveValue * stats.movementSpeed * Time.fixedDeltaTime);
    }
}
