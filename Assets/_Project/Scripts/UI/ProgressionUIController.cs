using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProgressionUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private VisualTreeAsset _abilityUnlockOption;

    private VisualElement _root;
    private VisualElement _abilityUnlockElement;

    private AbilityHelper _abilityHelper;
    private Character _playerObj;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _abilityUnlockElement = _root.Q<VisualElement>("AbilityUnlock");

        _abilityHelper = FindAnyObjectByType<AbilityHelper>();

        _abilityUnlockElement.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        
    }


    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;
    }
    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;

        _playerObj.OnAbilityUnlockTriggered -= HandleAbilityUnlock;
    }

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        _playerObj.OnAbilityUnlockTriggered += HandleAbilityUnlock;
    }

    private void HandleAbilityUnlock()
    {
        List<Ability> rolledAbilities = _abilityHelper.RollRandomAbilities(3);

        if(rolledAbilities == null)
        {
            Logger.Log("No abilities available in pool");
            return;
        }

        for(int i = 0; i < rolledAbilities.Count; i++)
        {
            VisualElement option = _abilityUnlockOption.CloneTree();

            Image image = option.Q<Image>("Image");
            Label name = option.Q<Label>("Name");
            Label affinity = option.Q<Label>("Affinity");
            Label multiplier = option.Q<Label>("Multiplier");
            Label cost = option.Q<Label>("Cost");
            Label cooldown = option.Q<Label>("Cooldown");
            Label description = option.Q<Label>("Description");
            Button choose = option.Q<Button>("Choose");

            image.image = rolledAbilities[i].Icon.texture;
            name.text = rolledAbilities[i].AbilityName;
            affinity.text = rolledAbilities[i].Affinity.ToString();
            multiplier.text = rolledAbilities[i].AttackMultiplier * 100 + "% of attack"; 
            cost.text = rolledAbilities[i].ManaCost.ToString() + " mana";
            cooldown.text = rolledAbilities[i].CooldownTime.ToString() + " secs";
            description.text = rolledAbilities[i].Description;

            choose.RegisterCallback<ClickEvent>(UnlockAbility);
            choose.dataSource = rolledAbilities[i];

            _abilityUnlockElement.Add(option);
        }

        _abilityUnlockElement.style.display = DisplayStyle.Flex;
    }

    private void UnlockAbility(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        Ability ability = button.dataSource as Ability;

        _playerObj.AddAbility(ability);

        button.UnregisterCallback<ClickEvent>(UnlockAbility);
        _abilityHelper.RemoveAbility(ability);

        _abilityUnlockElement.Clear();
        _abilityUnlockElement.style.display = DisplayStyle.None;
    }
}
