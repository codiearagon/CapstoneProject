using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityHelper : MonoBehaviour
{
    [SerializeField]
    private List<Ability> _abilities;

    public List<Ability> RollRandomAbilities(int amount)
    {
        if(_abilities.Count == 0)
            return null;

        amount = Mathf.Min(amount, _abilities.Count);

        List<Ability> pool = new List<Ability>(_abilities);
        List<Ability> rolledAbilities = new List<Ability>();

        for(int i = 0; i < amount; i++)
        {
            int randomInt = Random.Range(0, pool.Count);
            rolledAbilities.Add(pool[randomInt]);
            pool.RemoveAt(randomInt);
        }

        return rolledAbilities;
    }

    public void RemoveAbility(Ability ability)
    {
        int index = _abilities.FindIndex(a => a.Properties.Identifier == ability.Properties.Identifier);
        _abilities.RemoveAt(index);
    }
}
