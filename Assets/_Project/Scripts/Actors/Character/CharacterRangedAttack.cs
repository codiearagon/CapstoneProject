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

    private GameObject _projectilePrefab;
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

    private void Awake()
    {
        _projectilePrefab = _projectileAbility.ProjectilePrefab;
    }

    private void Start()
    {
        _character = GetComponentInParent<Character>();
        _rb = _character.GetComponent<Rigidbody2D>();

        _projectileAbility.SetBehaviour(new StraightProjectile(), new DamageOnHitProjectile());
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        Logger.Log("Fired");

        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(_character.LookValue) - _rb.position).normalized;

        _projectileAbility.SetDamage(CalculateDamage());
        _projectileAbility.SetDirection(direction);
        _projectileAbility.Cast(transform.parent.gameObject, LayerMask.NameToLayer("CharacterAttack"));
    }

    private float CalculateDamage()
    {
        float hpDamage = _character.Stats.MaxHp * _projectileAbility.HpMultiplier;
        float attackDamage = _character.Stats.Attack * _projectileAbility.AttackMultiplier;
        float defenseDamage = _character.Stats.Defense * _projectileAbility.DefenseMultiplier;

        float finalDamage = hpDamage + attackDamage + defenseDamage;

        return finalDamage;
    }
}
