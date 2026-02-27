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
    private VisualElement _abilityUpgradeElement;
    private VisualElement _advancementElement;

    private AbilityHelper _abilityHelper;
    private Character _playerObj;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _abilityUnlockElement = _root.Q<VisualElement>("AbilityUnlock");
        _abilityUpgradeElement = _root.Q<VisualElement>("AbilityUpgrade");
        _advancementElement = _root.Q<VisualElement>("Advancement");

        _abilityHelper = FindAnyObjectByType<AbilityHelper>();

        _abilityUnlockElement.style.display = DisplayStyle.None;
        _abilityUpgradeElement.style.display = DisplayStyle.None;
        _advancementElement.style.display = DisplayStyle.None;
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
        _playerObj.OnAbilityUpgradeTriggered -= HandleAbilityUpgrade;
    }

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        _playerObj.OnAbilityUnlockTriggered += HandleAbilityUnlock;
        _playerObj.OnAbilityUpgradeTriggered += HandleAbilityUpgrade;
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

            image.image = rolledAbilities[i].Properties.Icon.texture;
            name.text = rolledAbilities[i].Properties.AbilityName;
            affinity.text = rolledAbilities[i].Properties.Affinity.ToString();
            multiplier.text = rolledAbilities[i].Properties.AttackMultiplier * 100 + "% of attack"; 
            cost.text = rolledAbilities[i].Properties.ManaCost.ToString() + " mana";
            cooldown.text = rolledAbilities[i].Properties.CooldownTime.ToString() + " secs";
            description.text = rolledAbilities[i].Properties.Description;

            choose.RegisterCallback<ClickEvent>(UnlockAbility);
            choose.dataSource = rolledAbilities[i];

            _abilityUnlockElement.Add(option);
        }

        _abilityUnlockElement.style.display = DisplayStyle.Flex;
    }

    private void HandleAbilityUpgrade(List<Ability> abilities)
    {
        int amount = Mathf.Min(3, abilities.Count);

        for(int i = 0; i < amount; i++)
        {
            int randomIdx = Random.Range(0, abilities.Count);

            VisualElement option = _abilityUnlockOption.CloneTree();
            Ability ability = abilities[randomIdx];

            Image image = option.Q<Image>("Image");
            Label name = option.Q<Label>("Name");
            Label affinity = option.Q<Label>("Affinity");
            Label multiplier = option.Q<Label>("Multiplier");
            Label cost = option.Q<Label>("Cost");
            Label cooldown = option.Q<Label>("Cooldown");
            Label description = option.Q<Label>("Description");
            Button choose = option.Q<Button>("Choose");

            int upgradeIndex = ability.Level - 1;

            // if there are no available upgrades for this ability
            if (upgradeIndex >= ability.Upgrades.Count)
                continue;

            image.image = ability.Upgrades[upgradeIndex].Icon.texture;
            name.text = ability.Upgrades[upgradeIndex].AbilityName;
            affinity.text = ability.Upgrades[upgradeIndex].Affinity.ToString();
            multiplier.text = ability.Properties.AttackMultiplier * 100 + "% -> " + ability.Upgrades[upgradeIndex].AttackMultiplier * 100 + "% of attack";
            cost.text = ability.Properties.ManaCost + " -> " + ability.Upgrades[upgradeIndex].ManaCost.ToString() + " mana";
            cooldown.text = ability.Properties.CooldownTime + " -> " + ability.Upgrades[upgradeIndex].CooldownTime.ToString() + " secs";
            description.text = ability.Upgrades[upgradeIndex].Description;

            choose.RegisterCallback<ClickEvent>(UnlockUpgrade);
            choose.dataSource = ability.Upgrades[upgradeIndex];

            _abilityUpgradeElement.Add(option);
        }

        if (_abilityUpgradeElement.childCount == 0)
            return;

        _abilityUpgradeElement.style.display = DisplayStyle.Flex;
    }

    private void UnlockAbility(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        Ability ability = button.dataSource as Ability;

        Ability instance = Instantiate(ability, _playerObj.transform);
        _playerObj.AddAbility(instance);

        button.UnregisterCallback<ClickEvent>(UnlockAbility);
        _abilityHelper.RemoveAbility(ability);

        _abilityUnlockElement.Clear();
        _abilityUnlockElement.style.display = DisplayStyle.None;
    }

    private void UnlockUpgrade(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        AbilityProperties abilityProps = button.dataSource as AbilityProperties;

        _playerObj.UpgradeAbility(abilityProps);

        button.UnregisterCallback<ClickEvent>(UnlockUpgrade);

        _abilityUpgradeElement.Clear();
        _abilityUpgradeElement.style.display = DisplayStyle.None;
    }
}
