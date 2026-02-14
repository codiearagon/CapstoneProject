using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRangedAttack : MonoBehaviour
{
    [SerializeField]
    private ProjectileAbility _projectileAbility;

    [SerializeField]
    private InputActionReference _attackRef;

    private Character _character;
    private Rigidbody2D _rb;

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
        _rb = _character.GetComponent<Rigidbody2D>();

        _character.AddAbility(_projectileAbility);
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        if (_character.Stats.CurrentMana < _projectileAbility.ManaCost)
        {
            Logger.Log("Not enough mana");
            return;
        }

        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(_character.LookValue) - _rb.position).normalized;

        _projectileAbility.SetData(CalculateDamage(), direction);
        _projectileAbility.Cast(transform.parent.gameObject, LayerMask.NameToLayer("CharacterAttack"));

        _character.UseMana(_projectileAbility.ManaCost);
    }

    private float CalculateDamage()
    {
        float attackDamage = _character.Stats.Attack * _projectileAbility.AttackMultiplier;

        float finalDamage = attackDamage;

        return finalDamage;
    }
}
