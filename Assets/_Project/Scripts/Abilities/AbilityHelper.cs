using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityHelper : MonoBehaviour
{
    [SerializeField]
    private List<Ability> abilities;

    public List<Ability> RollRandomAbilities(int amount)
    {
        if(abilities.Count == 0)
            return null;

        List<Ability> rolledAbilities = new List<Ability>();

        for(int i = 0; i < amount; i++)
        {
            int randomInt = Random.Range(0, abilities.Count);
            rolledAbilities.Add(abilities[randomInt]);
        }

        return rolledAbilities;
    }

    public void RemoveAbility(Ability ability)
    {
        int index = abilities.FindIndex(a => a.Properties.AbilityName == ability.Properties.AbilityName);
        abilities.RemoveAt(index);
    }
}
