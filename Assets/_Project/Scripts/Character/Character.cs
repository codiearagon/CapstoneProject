using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static event Action<Stats> OnStatsChanged;

    public CharacterBaseSO BaseData { get; private set; }
    public Stats Stats { get; private set; }

    private Rigidbody2D _rb;

    // Gets ran by PlayerInitialize
    public void Initialize(CharacterBaseSO baseData)
    {
        Stats = new Stats();
        BaseData = baseData;
        Stats.InitializeStats(BaseData);

        // component initialization
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        OnStatsChanged?.Invoke(Stats);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void Move(Vector2 moveDir)
    {
        _rb.MovePosition(_rb.position + moveDir * Stats.MovementSpeed * Time.fixedDeltaTime);
    }

    public void Look(Vector2 lookDir)
    {

    }
}
