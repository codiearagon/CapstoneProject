using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class Character : MonoBehaviour
{
    public static event Action<CharacterStats> OnStatsChanged;

    public CharacterStats Stats => _stats;

    [Header("References")]
    [SerializeField]
    private InputActionReference _moveRef;

    [Header("Properties")]
    [SerializeField]
    private CharacterStats _stats;

    private Rigidbody2D _rb;

    private Vector2 _moveValue;

    private void OnEnable()
    {
        _moveRef.action.Enable();
    }

    private void OnDisable()
    {
        _moveRef.action.Disable();
    }

    private void Awake()
    {
        _stats = new CharacterStats();
        _rb = GetComponent<Rigidbody2D>();

        _stats.CurrentHp = _stats.MaxHp;
    }

    private void Start()
    {
        OnStatsChanged?.Invoke(_stats);
    }

    private void Update()
    {
        _moveValue = _moveRef.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveValue * _stats.MovementSpeed * Time.deltaTime);
    }

    public void TakeDamage(int amount)
    {
        _stats.CurrentHp = Mathf.Clamp(_stats.CurrentHp - amount, 0, _stats.MaxHp);
    }
}
