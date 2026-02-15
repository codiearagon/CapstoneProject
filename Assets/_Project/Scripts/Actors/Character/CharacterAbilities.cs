using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAbilities : MonoBehaviour
{
    private List<Ability> _abilities;

    private PlayerInput _input;
    private Character _character;
    private Rigidbody2D _rb;

    private void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Ability1.performed += OnAbility1;
        _input.Player.Ability2.performed += OnAbility2;
        _input.Player.Ability3.performed += OnAbility3;
        _input.Player.Ability4.performed += OnAbility4;
        _input.Player.Ability5.performed += OnAbility5;
        _input.Player.Ability6.performed += OnAbility6;
        _input.Player.Ability7.performed += OnAbility7;
        _input.Player.Ability8.performed += OnAbility8;
    }

    private void OnDisable()
    {
        _input.Player.Disable();
        _input.Player.Ability1.performed -= OnAbility1;
        _input.Player.Ability2.performed -= OnAbility2;
        _input.Player.Ability3.performed -= OnAbility3;
        _input.Player.Ability4.performed -= OnAbility4;
        _input.Player.Ability5.performed -= OnAbility5;
        _input.Player.Ability6.performed -= OnAbility6;
        _input.Player.Ability7.performed -= OnAbility7;
        _input.Player.Ability8.performed -= OnAbility8;
    }

    private void Awake()
    {
        _input = new PlayerInput();
        _abilities = new List<Ability>();
    }

    private void Start()
    {
        _character = GetComponentInParent<Character>();
        _rb = _character.GetComponent<Rigidbody2D>();
    }

    private void OnAbility1(InputAction.CallbackContext ctx) { CastAbility(0); }
    private void OnAbility2(InputAction.CallbackContext ctx) { CastAbility(1); }
    private void OnAbility3(InputAction.CallbackContext ctx) { CastAbility(2); }
    private void OnAbility4(InputAction.CallbackContext ctx) { CastAbility(3); }
    private void OnAbility5(InputAction.CallbackContext ctx) { CastAbility(4); }
    private void OnAbility6(InputAction.CallbackContext ctx) { CastAbility(5); }
    private void OnAbility7(InputAction.CallbackContext ctx) { CastAbility(6); }
    private void OnAbility8(InputAction.CallbackContext ctx) { CastAbility(7); }

    private void CastAbility(int index)
    {
        if (index >= _abilities.Count)
            return;

        if (_character.Stats.CurrentMana < _abilities[index].ManaCost)
        {
            Logger.Log("Not enough mana");
            return;
        }

        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(_character.LookValue) - _rb.position).normalized;

        _abilities[index].SetRuntimeData(CalculateDamage(_abilities[index]), direction);
        _abilities[index].Cast(transform.parent.gameObject);

        _character.UseMana(_abilities[index].ManaCost);
    }

    private float CalculateDamage(Ability ability)
    {
        float attackDamage = _character.Stats.Attack * ability.AttackMultiplier;
        float affinityMultiplier = Utility.GetMultiplier(_character.Stats, ability.Affinity);
        float finalDamage = attackDamage * affinityMultiplier;

        return finalDamage;
    }

    public void AddAbility(Ability ability)
    {
        ability.SetLayer(transform.parent.gameObject.layer);
        _abilities.Add(ability);
    }

    public List<Ability> GetList()
    {
        return _abilities;
    }
}
