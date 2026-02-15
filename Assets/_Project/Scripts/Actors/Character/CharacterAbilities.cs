using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAbilities : MonoBehaviour
{
    private List<Ability> _abilities;

    private PlayerInput _input;
    private Character _character;

    private void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Ability1.performed += OnAbility1;
    }

    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        _input = new PlayerInput();
        _abilities = new List<Ability>();
    }

    private void Start()
    {
        _character = GetComponentInParent<Character>();
    }

    private void OnAbility1(InputAction.CallbackContext ctx)
    {
        _abilities[0].Cast(transform.parent.gameObject);
    }
}
