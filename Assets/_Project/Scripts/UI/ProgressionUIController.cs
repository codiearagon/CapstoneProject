using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProgressionUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerRoot _playerRoot;

    [SerializeField]
    private VisualTreeAsset _abilityUnlockOption;

    [SerializeField]
    private VisualTreeAsset _advancementOption;

    private VisualElement _root;
    private VisualElement _abilityUnlockElement;
    private VisualElement _abilityUpgradeElement;
    private VisualElement _advancementElement;
    private VisualElement _optionContainer;

    private Button _prevButton;
    private Button _nextButton;
    private Button _detailsButton;
    private Button _chooseButton;

    private List<VisualElement> _characterAdvancements;

    private VisualElement _selectedAdvancement;
    private int _selectedIndex;

    private AbilityHelper _abilityHelper;
    private Character _playerObj;

    private void Awake()
    {
        _characterAdvancements = new List<VisualElement>();

        _root = GetComponent<UIDocument>().rootVisualElement;
        _abilityUnlockElement = _root.Q<VisualElement>("AbilityUnlock");
        _abilityUpgradeElement = _root.Q<VisualElement>("AbilityUpgrade");
        _advancementElement = _root.Q<VisualElement>("Advancement");
        _optionContainer = _advancementElement.Q<VisualElement>("OptionContainer");

        _prevButton = _advancementElement.Q<Button>("PreviousButton");
        _nextButton = _advancementElement.Q<Button>("NextButton");
        _detailsButton = _advancementElement.Q<Button>("DetailsButton");
        _chooseButton = _advancementElement.Q<Button>("ChooseButton");

        _abilityHelper = FindAnyObjectByType<AbilityHelper>();

        _abilityUnlockElement.style.display = DisplayStyle.None;
        _abilityUpgradeElement.style.display = DisplayStyle.None;
        _advancementElement.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;

        _prevButton.RegisterCallback<ClickEvent>(PreviousAdvancement);
        _nextButton.RegisterCallback<ClickEvent>(NextAdvancement);
        _detailsButton.RegisterCallback<ClickEvent>(ShowDetails);
        _chooseButton.RegisterCallback<ClickEvent>(ChooseAdvancement);
    }
    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;

        _playerObj.OnAbilityUnlockTriggered -= HandleAbilityUnlock;
        _playerObj.OnAbilityUpgradeTriggered -= HandleAbilityUpgrade;
        _playerObj.OnAdvancementTriggered -= HandleAdvancement;

        _prevButton.UnregisterCallback<ClickEvent>(PreviousAdvancement);
        _nextButton.UnregisterCallback<ClickEvent>(NextAdvancement);
        _detailsButton.UnregisterCallback<ClickEvent>(ShowDetails);
        _chooseButton.UnregisterCallback<ClickEvent>(ChooseAdvancement);
    }

    private void HandlePlayerSpawned(GameObject player)
    {
        _playerObj = player.GetComponent<Character>();

        _playerObj.OnAbilityUnlockTriggered += HandleAbilityUnlock;
        _playerObj.OnAbilityUpgradeTriggered += HandleAbilityUpgrade;
        _playerObj.OnAdvancementTriggered += HandleAdvancement;
    }

    private void HandleAbilityUnlock()
    {
        Utility.RequestPause();
        List<Ability> rolledAbilities = _abilityHelper.RollRandomAbilities(3);

        if(rolledAbilities == null)
        {
            Logger.Log("No abilities available in pool");
            Utility.ReleasePause();
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
        Utility.RequestPause();
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
        {
            Utility.ReleasePause();
            return;
        }

        _abilityUpgradeElement.style.display = DisplayStyle.Flex;
    }

    private void HandleAdvancement(List<CharacterAdvancement> list)
    {
        if (list.Count == 0)
            return;

        Utility.RequestPause();

        foreach (CharacterAdvancement advancement in list)
        {
            VisualElement option = _advancementOption.CloneTree();
            option.dataSource = advancement;

            Image splash = option.Q<Image>("SplashImage");
            Label name = option.Q<Label>("Name");
            Label affinity = option.Q<Label>("Affinity");
            Label description = option.Q<Label>("Description");
            Label statSummary = option.Q<Label>("StatSummary");
            VisualElement abilities = option.Q<VisualElement>("AbilitiesContainer");

            splash.image = advancement.SplashArt.texture;
            name.text = advancement.AdvancementName;
            affinity.text = advancement.Affinity.ToString();
            description.text = advancement.Description;
            statSummary.text = advancement.StatSummary;

            foreach (Ability ability in advancement.Abilities)
            {
                Image ab = new Image();
                ab.image = ability.Properties.Icon.texture;

                abilities.Add(ab);
            }

            _characterAdvancements.Add(option);
        }

        _selectedIndex = 0;
        _selectedAdvancement = _characterAdvancements[_selectedIndex];
        _optionContainer.Add(_selectedAdvancement);

        _advancementElement.style.display = DisplayStyle.Flex;
    }

    private void PreviousAdvancement(ClickEvent evt)
    {
        if (_selectedIndex == 0)
            _selectedIndex = _characterAdvancements.Count - 1;
        else
            _selectedIndex--;

        ResetStyle();
        _selectedAdvancement = _characterAdvancements[_selectedIndex];
        _optionContainer.Clear();
        _optionContainer.Add(_selectedAdvancement);
    }

    private void NextAdvancement(ClickEvent evt)
    {
        if (_selectedIndex == _characterAdvancements.Count - 1)
            _selectedIndex = 0;
        else
            _selectedIndex++;

        ResetStyle();
        _selectedAdvancement = _characterAdvancements[_selectedIndex];
        _optionContainer.Clear();
        _optionContainer.Add(_selectedAdvancement);
    }

    private void ShowDetails(ClickEvent evt)
    {
        VisualElement _detailsContainer = _selectedAdvancement.Q<VisualElement>("DetailsContainer");
        VisualElement _imageContainer = _selectedAdvancement.Q<VisualElement>("ImageContainer");

        if (_detailsContainer.style.display == DisplayStyle.Flex)
        {
            _detailsButton.text = "Show Details";
            _detailsContainer.style.display = DisplayStyle.None;
            _imageContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
            _detailsButton.text = "Hide Details";
            _detailsContainer.style.display = DisplayStyle.Flex;
            _imageContainer.style.display = DisplayStyle.None;
        }
    }

    private void ChooseAdvancement(ClickEvent evt)
    {
        _playerObj.SelectAdvancement(_selectedAdvancement.dataSource as CharacterAdvancement);
        _characterAdvancements.Clear();
        _selectedAdvancement.Clear();
        _optionContainer.Clear();
        _advancementElement.style.display = DisplayStyle.None;
        Utility.ReleasePause();
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

        Utility.ReleasePause();
    }

    private void UnlockUpgrade(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        AbilityProperties abilityProps = button.dataSource as AbilityProperties;

        _playerObj.UpgradeAbility(abilityProps);

        button.UnregisterCallback<ClickEvent>(UnlockUpgrade);

        _abilityUpgradeElement.Clear();
        _abilityUpgradeElement.style.display = DisplayStyle.None;

        Utility.ReleasePause();
    }

    private void ResetStyle()
    {
        VisualElement _detailsContainer = _selectedAdvancement.Q<VisualElement>("DetailsContainer");
        VisualElement _imageContainer = _selectedAdvancement.Q<VisualElement>("ImageContainer");

        _detailsContainer.style.display = DisplayStyle.None;
        _imageContainer.style.display = DisplayStyle.Flex;

        _detailsButton.text = "Show Details";
    }
}
