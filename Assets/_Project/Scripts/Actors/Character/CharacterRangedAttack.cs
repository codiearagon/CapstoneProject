using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRangedAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectilePrefab;

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
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(_character.LookValue) - _rb.position).normalized;

        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);

        float damage = CalculateDamage();
        projectile.GetComponent<Projectile>().SetProperties(damage, direction, _character.Stats.Affinity, LayerMask.NameToLayer("CharacterAttack"));
    }

    private float CalculateDamage()
    {
        float hpBaseDamage = _character.Stats.HpScaling * _character.Stats.MaxHp;
        float attackBaseDamage = _character.Stats.AttackScaling * _character.Stats.Attack;
        float defenseBaseDamage = _character.Stats.DefenseScaling * _character.Stats.Defense;

        float baseDamage = hpBaseDamage + attackBaseDamage + defenseBaseDamage;

        return baseDamage;
    }
}
