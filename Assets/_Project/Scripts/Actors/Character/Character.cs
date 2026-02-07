using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour, IDamageable
{
    public static event Action<CharacterStats> OnStatsChanged;

    [Header("References")]
    [SerializeField]
    private InputActionReference _moveRef;

    [SerializeField]
    private InputActionReference _lookRef;

    [Header("Properties")]
    [SerializeField]
    private CharacterStats _stats;

    private Rigidbody2D _rb;

    private Vector2 _moveValue;
    private Vector2 _lookValue;

    private void OnEnable()
    {
        _moveRef.action.Enable();
        _lookRef.action.Enable();
    }

    private void OnDisable()
    {
        _moveRef.action.Disable();
        _lookRef.action.Disable();
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

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


    private void LevelUp()
    {
        _stats.Level++;
        _stats.ExpToLevelUp += _stats.ExpToLevelUp * (2 / 1.5f);
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

        OnStatsChanged?.Invoke(_stats);
    }

    public void ReceiveExperience(float amount)
    {
        _stats.CurrentExp += amount;

        Logger.Log("Current Exp: " + _stats.CurrentExp + ", Level: " + _stats.Level);

        if (_stats.CurrentExp >= _stats.ExpToLevelUp)
            LevelUp();
    }

    public CharacterStats Stats => _stats;
    public Vector2 LookValue => _lookValue;
}
