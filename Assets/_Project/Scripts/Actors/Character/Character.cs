using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IAttackBehaviour))]
public class Character : MonoBehaviour, IDamageable
{
    public static event Action<CharacterStats> OnStatsChanged;

    public CharacterStats Stats => _stats;

    [Header("References")]
    [SerializeField]
    private InputActionReference _moveRef;

    [SerializeField]
    private InputActionReference _lookRef;

    [SerializeField]
    private InputActionReference _attackRef;

    [Header("Properties")]
    [SerializeField]
    private CharacterStats _stats;

    private Rigidbody2D _rb;
    private IAttackBehaviour _attackBehaviour;

    private Vector2 _moveValue;
    private Vector2 _lookValue;

    private void OnEnable()
    {
        _moveRef.action.Enable();
        _lookRef.action.Enable();
        _attackRef.action.Enable();

        _attackRef.action.performed += Attack;
    }

    private void OnDisable()
    {
        _moveRef.action.Disable();
        _lookRef.action.Disable();
        _attackRef.action.Disable();

        _attackRef.action.performed -= Attack;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _attackBehaviour = GetComponent<IAttackBehaviour>();

        _stats.CurrentHp = _stats.MaxHp;

        Logger.Log("Character Initialized");
    }

    private void Start()
    {
        OnStatsChanged?.Invoke(_stats);
    }

    private void Update()
    {
        _lookValue = _lookRef.action.ReadValue<Vector2>();
        _moveValue = _moveRef.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveValue * (_stats.MovementSpeed / 10) * Time.deltaTime);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Logger.Log("Player triggered attack");

        Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(_lookValue) - _rb.position;

        Logger.Log("Projectile dir " + direction);

        _attackBehaviour.Attack(_stats.Attack, direction, _stats.Affinity);
    }

    public void TakeDamage(float amount, Affinity damageAffinity)
    {
        float affinityMultiplier = AffinityLookup.GetMultiplier(damageAffinity, _stats.Affinity);
        float finalDamage = amount * affinityMultiplier;
        Logger.Log(string.Format("Received Damage: {0}, {1} base * {2}, {3}", finalDamage, amount, affinityMultiplier, _stats.CharacterName));

        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - finalDamage, 0, _stats.MaxHp);

        Color objCol = GetComponent<SpriteRenderer>().color;
        objCol.a = (_stats.CurrentHp) / (_stats.MaxHp);

        GetComponent<SpriteRenderer>().color = objCol;

        OnStatsChanged?.Invoke(Stats);
    }
}
