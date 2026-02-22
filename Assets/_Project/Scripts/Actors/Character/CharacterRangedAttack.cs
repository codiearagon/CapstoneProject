using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRangedAttack : MonoBehaviour
{
    [SerializeField]
    private Ability _ability;

    private PlayerInput _input;
    private Character _character;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Attack.started += OnAttackStarted;
        _input.Player.Attack.canceled += OnAttackCancelled;
    }

    private void OnDisable()
    {
        _input.Player.Disable();
        _input.Player.Attack.started -= OnAttackStarted;
        _input.Player.Attack.canceled -= OnAttackCancelled;
    }


    private void Start()
    {
        _character = GetComponentInParent<Character>();
        _rb = _character.GetComponent<Rigidbody2D>();
        _ability.SetLayer(transform.parent.gameObject.layer);
    }

    private void OnAttackStarted(InputAction.CallbackContext ctx)
    {
        if (_character.Stats.CurrentMana < _ability.Properties.ManaCost)
        {
            Logger.Log("Not enough mana");
            return;
        }

        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(_character.LookValue) - _rb.position).normalized;

        _ability.SetRuntimeData(CalculateDamage(), direction);
        _ability.Cast(transform.parent.gameObject);

        _character.UseMana(_ability.Properties.ManaCost);
    }

    private void OnAttackCancelled(InputAction.CallbackContext ctx)
    {
        
    }

    private float CalculateDamage()
    {
        float attackDamage = _character.Stats.Attack * _ability.Properties.AttackMultiplier;
        float affinityMultiplier = Utility.GetMultiplier(_character.Stats, _ability.Properties.Affinity);
        float finalDamage = attackDamage * affinityMultiplier;

        return finalDamage;
    }
}
