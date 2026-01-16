using UnityEngine;

public class CharacterStats : MonoBehaviour
{


    [field: SerializeField]
    public CharacterBaseSO baseData { get; private set; }

    // realtime stats
    public int maxHp { get; private set; }
    public int currentHp { get; private set; }
    public int movementSpeed { get; private set; }
    public int attack { get; private set; }
    public int attackSpeed { get; private set; }
    public int defense { get; private set; }

    private void Start()
    {
        maxHp = baseData.hp;
        currentHp = baseData.hp;
        movementSpeed = baseData.movementSpeed;
        attack = baseData.attack;
        attackSpeed = baseData.attackSpeed;
        defense = baseData.defense;
    }

    private void Update()
    {
        
    }
}
