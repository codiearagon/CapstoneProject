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
    private VisualElement _abilityUnlockContainer;
    private VisualElement _abilityUpgradeElement;
    private VisualElement _abilityUpgradeContainer;
    private VisualElement _advancementElement;
    private VisualElement _optionContainer;
    private VisualElement _dimmer;
    private Image _advancementBg;

    private Button _prevButton;
    private Button _nextButton;

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
        _abilityUnlockContainer = _root.Q<VisualElement>("AbilityUnlockContainer");
        _abilityUpgradeElement = _root.Q<VisualElement>("AbilityUpgrade");
        _abilityUpgradeContainer = _root.Q<VisualElement>("AbilityUpgradeContainer");
        _dimmer = _root.Q<VisualElement>("Dimmer");
        _advancementElement = _root.Q<VisualElement>("Advancement");
        _optionContainer = _advancementElement.Q<VisualElement>("OptionContainer");

        _advancementBg = _advancementElement.Q<Image>("Background");
        _prevButton = _advancementElement.Q<Button>("PreviousButton");
        _nextButton = _advancementElement.Q<Button>("NextButton");

        _abilityHelper = FindAnyObjectByType<AbilityHelper>();

        _abilityUnlockElement.style.display = DisplayStyle.None;
        _abilityUpgradeElement.style.display = DisplayStyle.None;
        _advancementElement.style.display = DisplayStyle.None;
        _dimmer.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _playerRoot.OnPlayerSpawned += HandlePlayerSpawned;

        _prevButton.RegisterCallback<ClickEvent>(PreviousAdvancement);
        _nextButton.RegisterCallback<ClickEvent>(NextAdvancement);
    }
    private void OnDisable()
    {
        _playerRoot.OnPlayerSpawned -= HandlePlayerSpawned;

        _playerObj.OnAbilityUnlockTriggered -= HandleAbilityUnlock;
        _playerObj.OnAbilityUpgradeTriggered -= HandleAbilityUpgrade;
        _playerObj.OnAdvancementTriggered -= HandleAdvancement;

        _prevButton.UnregisterCallback<ClickEvent>(PreviousAdvancement);
        _nextButton.UnregisterCallback<ClickEvent>(NextAdvancement);
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

            option.style.width = Length.Percent(100);
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
            affinity.style.color = Utility.GetAffinityColor(rolledAbilities[i].Properties.Affinity);
            multiplier.text = rolledAbilities[i].Properties.AttackMultiplier * 100 + "% of attack"; 
            cost.text = rolledAbilities[i].Properties.ManaCost.ToString() + " mana";
            cooldown.text = rolledAbilities[i].Properties.CooldownTime.ToString() + " secs";
            description.text = rolledAbilities[i].Properties.Description;

            choose.style.backgroundColor = Utility.GetAffinityColor(rolledAbilities[i].Properties.Affinity);
            choose.RegisterCallback<ClickEvent>(UnlockAbility);
            choose.dataSource = rolledAbilities[i];

            _abilityUnlockContainer.Add(option);
        }

        _dimmer.style.display = DisplayStyle.Flex;
        _abilityUnlockElement.style.display = DisplayStyle.Flex;
    }

    private void HandleAbilityUpgrade(List<Ability> abilities)
    {
        Utility.RequestPause();
        int amount = Mathf.Min(3, abilities.Count);
        List<Ability> pool = new List<Ability>(abilities);

        for(int i = 0; i < amount; i++)
        {
            int randomIdx = Random.Range(0, pool.Count);

            VisualElement option = _abilityUnlockOption.CloneTree();
            Ability ability = pool[randomIdx];

            option.style.width = Length.Percent(100);
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
            affinity.style.color = Utility.GetAffinityColor(ability.Properties.Affinity);
            multiplier.text = ability.Properties.AttackMultiplier * 100 + "% -> " + ability.Upgrades[upgradeIndex].AttackMultiplier * 100 + "% of attack";
            cost.text = ability.Properties.ManaCost + " -> " + ability.Upgrades[upgradeIndex].ManaCost.ToString() + " mana";
            cooldown.text = ability.Properties.CooldownTime + " -> " + ability.Upgrades[upgradeIndex].CooldownTime.ToString() + " secs";
            description.text = ability.Upgrades[upgradeIndex].UpgradeDescription;

            choose.style.backgroundColor = Utility.GetAffinityColor(ability.Properties.Affinity);
            choose.RegisterCallback<ClickEvent>(UnlockUpgrade);
            choose.dataSource = ability.Upgrades[upgradeIndex];

            _abilityUpgradeContainer.Add(option);
            pool.RemoveAt(randomIdx);
        }

        if (_abilityUpgradeElement.childCount == 0)
        {
            Utility.ReleasePause();
            return;
        }

        _dimmer.style.display = DisplayStyle.Flex;
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
            option.style.position = Position.Absolute;
            option.style.width = Length.Percent(100);
            option.style.height = Length.Percent(100);

            Label name = option.Q<Label>("Name");
            Label affinity = option.Q<Label>("Affinity");
            Label description = option.Q<Label>("Description");
            Label statSummary = option.Q<Label>("StatSummary");
            VisualElement statsContainer = option.Q<VisualElement>("StatsContainer");
            VisualElement abilities = option.Q<VisualElement>("AbilitiesContainer");
            Button choose = option.Q<Button>("Choose");

            name.text = advancement.AdvancementName;
            affinity.text = advancement.Affinity.ToString();
            affinity.style.color = Utility.GetAffinityColor(advancement.Affinity);
            description.text = advancement.Description;
            statSummary.text = advancement.StatSummary;

            VisualElement stats = option.Q<VisualElement>("Stats");
            stats.style.borderBottomColor = Utility.GetAffinityColor(advancement.Affinity);
            stats.style.borderTopColor = Utility.GetAffinityColor(advancement.Affinity);
            stats.style.borderLeftColor = Utility.GetAffinityColor(advancement.Affinity);
            stats.style.borderRightColor = Utility.GetAffinityColor(advancement.Affinity);

            if (advancement.MaxHp > 0) statsContainer.Add(new Label($"+{advancement.MaxHp} Max HP"));
            if (advancement.HpRegenRate > 0) statsContainer.Add(new Label($"+{advancement.HpRegenRate} HP Regeneration"));
            if (advancement.MaxMana > 0) statsContainer.Add(new Label($"+{advancement.MaxMana} Max Mana"));
            if (advancement.ManaRegenRate > 0) statsContainer.Add(new Label($"+{advancement.ManaRegenRate} Mana Regeneration"));
            if (advancement.MovementSpeed > 0) statsContainer.Add(new Label($"+{advancement.MovementSpeed} Movement Speed"));
            if (advancement.Attack > 0) statsContainer.Add(new Label($"+{advancement.Attack} Attack"));
            if (advancement.AttackSpeed > 0) statsContainer.Add(new Label($"+{advancement.AttackSpeed} Attack Speed"));
            if (advancement.Defense > 0) statsContainer.Add(new Label($"+{advancement.Defense} Defense"));
            if (advancement.FireMultiplier > 0) statsContainer.Add(new Label($"+{advancement.FireMultiplier * 100}% Fire Damage"));
            if (advancement.WaterMultiplier > 0) statsContainer.Add(new Label($"+{advancement.WaterMultiplier * 100}% Water Damage"));
            if (advancement.AirMultiplier > 0) statsContainer.Add(new Label($"+{advancement.AirMultiplier * 100}% Air Damage"));
            if (advancement.EarthMultiplier > 0) statsContainer.Add(new Label($"+{advancement.EarthMultiplier * 100}% Earth Damage"));
            if (advancement.DarkMultiplier > 0) statsContainer.Add(new Label($"+{advancement.DarkMultiplier * 100}% Dark Damage"));
            if (advancement.LightMultiplier > 0) statsContainer.Add(new Label($"+{advancement.LightMultiplier * 100}% Light Damage"));

            //foreach (Ability ability in advancement.Abilities)
            //{
            //    Image ab = new Image();
            //    ab.image = ability.Properties.Icon.texture;

            //    abilities.Add(ab);
            //}

            choose.style.backgroundColor = Utility.GetAffinityColor(advancement.Affinity);
            choose.RegisterCallback<ClickEvent>(ChooseAdvancement);
            _characterAdvancements.Add(option);
        }

        _selectedIndex = 0;
        _selectedAdvancement = _characterAdvancements[_selectedIndex];
        _optionContainer.Add(_selectedAdvancement);
        SetBg();
        _advancementElement.style.display = DisplayStyle.Flex;
    }

    private void PreviousAdvancement(ClickEvent evt)
    {
        if (_selectedIndex == 0)
            _selectedIndex = _characterAdvancements.Count - 1;
        else
            _selectedIndex--;

        _selectedAdvancement = _characterAdvancements[_selectedIndex];
        _optionContainer.Clear();
        _optionContainer.Add(_selectedAdvancement);
        SetBg();
    }

    private void NextAdvancement(ClickEvent evt)
    {
        if (_selectedIndex == _characterAdvancements.Count - 1)
            _selectedIndex = 0;
        else
            _selectedIndex++;

        _selectedAdvancement = _characterAdvancements[_selectedIndex];
        _optionContainer.Clear();
        _optionContainer.Add(_selectedAdvancement);
        SetBg();
    }

    private void SetBg()
    {
        _advancementBg.image = (_selectedAdvancement.dataSource as CharacterAdvancement).SplashArt.texture;
        _advancementBg.scaleMode = ScaleMode.ScaleAndCrop;
    }

    private void ChooseAdvancement(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        _playerObj.SelectAdvancement(_selectedAdvancement.dataSource as CharacterAdvancement);

        button.UnregisterCallback<ClickEvent>(ChooseAdvancement);
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

        _abilityUnlockContainer.Clear();
        _abilityUnlockElement.style.display = DisplayStyle.None;
        _dimmer.style.display = DisplayStyle.None;

        Utility.ReleasePause();
    }

    private void UnlockUpgrade(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        AbilityProperties abilityProps = button.dataSource as AbilityProperties;

        _playerObj.UpgradeAbility(abilityProps);

        button.UnregisterCallback<ClickEvent>(UnlockUpgrade);

        _abilityUpgradeContainer.Clear();
        _abilityUpgradeElement.style.display = DisplayStyle.None;
        _dimmer.style.display = DisplayStyle.None;

        Utility.ReleasePause();
    }
}
