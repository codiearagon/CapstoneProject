using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMeleeAttack : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _attackRef;

    private Character _character;

    private void OnEnable()
    {
        _attackRef.action.Enable();
        _attackRef.action.performed += AttackPerformed;
    }

    private void OnDisable()
    {
        _attackRef.action.Disable();
        _attackRef.action.performed -= AttackPerformed;
    }

    private void Start()
    {
        _character = GetComponentInParent<Character>();
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        
    }
}
