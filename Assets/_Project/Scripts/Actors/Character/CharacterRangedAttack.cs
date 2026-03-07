using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRangedAttack : MonoBehaviour
{
    [SerializeField]
    private Ability _ability;

    private Ability _runtimeAbility;
    private PlayerInput _input;
    private Character _character;
    private Rigidbody2D _rb;

    private bool _isAttacking;
    private Coroutine _attackCoroutine;

    private void Awake()
    {
        _input = new PlayerInput();
        _isAttacking = false;
    }

    private void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Attack.performed += OnAttackPerformed;
        _input.Player.Attack.canceled += OnAttackCancelled;
    }

    private void OnDisable()
    {
        _input.Player.Disable();
        _input.Player.Attack.performed -= OnAttackPerformed;
        _input.Player.Attack.canceled -= OnAttackCancelled;

        _character.OnAdvancementChosen -= HandleAdvancementChosen;

        _isAttacking = false;
    }


    private void Start()
    {
        _character = GetComponentInParent<Character>();
        _rb = _character.GetComponent<Rigidbody2D>();

        _character.OnAdvancementChosen += HandleAdvancementChosen;

        _runtimeAbility = Instantiate(_ability, transform);
        _runtimeAbility.SetLayer(transform.parent.gameObject.layer);
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        _isAttacking = true;

        if (_attackCoroutine == null)
            _attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    private void OnAttackCancelled(InputAction.CallbackContext ctx)
    {
        _isAttacking = false;
    }

    private IEnumerator AttackCoroutine()
    {
        while(true)
        {
            Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(_character.LookValue) - _rb.position).normalized;

            _runtimeAbility.SetRuntimeData(CalculateDamage(), direction);
            _runtimeAbility.Cast(transform.parent.gameObject);

            yield return new WaitForSeconds(1f / _character.Stats.AttackSpeed);

            if (!_isAttacking)
            {
                _attackCoroutine = null;
                yield break;
            }
        }
    }

    private void HandleAdvancementChosen(CharacterAdvancement advancement)
    {
        _runtimeAbility.ChangeAffinity(advancement.Affinity);
    }

    private float CalculateDamage()
    {
        float attackDamage = _character.Stats.Attack * _ability.Properties.AttackMultiplier;
        float affinityMultiplier = Utility.GetMultiplier(_character.Stats, _ability.Properties.Affinity);
        float finalDamage = attackDamage * affinityMultiplier;

        return finalDamage;
    }

    
}
