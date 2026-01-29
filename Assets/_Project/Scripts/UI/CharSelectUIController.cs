using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharSelectUIController : MonoBehaviour
{
    [SerializeField]
    private List<CharacterBaseSO> playableCharacters;

    private CharacterBaseSO selectedCharacter;

    private VisualElement _root;
    private VisualElement _charSelectElement;

    private ScrollView _charScrollView;
    private Image _charSplashArtImage;
    private Label _charNameLabel;
    private Label _charStatsLabel;

    private void Awake()
    {
        ResolveReferences();
        SetupScrollView();

        selectedCharacter = playableCharacters[0];
        UpdateSelectedCharacterUI();
    }

    public void OpenCharSelect()
    {
        _charSelectElement.style.display = DisplayStyle.Flex;
    }

    public void CloseCharSelect()
    {
        _charSelectElement.style.display = DisplayStyle.None;
    }

    private void ResolveReferences()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _charSelectElement = _root.Q<VisualElement>("CharSelectElement");

        _charScrollView = _charSelectElement.Q<ScrollView>("CharScrollView");
        _charSplashArtImage = _charSelectElement.Q<Image>("CharSplashArtImage");
        _charNameLabel = _charSelectElement.Q<Label>("CharNameLabel");
        _charStatsLabel = _charSelectElement.Q<Label>("CharStatsLabel");
    }

    private void SetupScrollView()
    {
        foreach(CharacterBaseSO character in playableCharacters)
        {
            Button iconButton = new Button();
            Image iconImage = new Image();
            iconImage.sprite = character.icon;
            iconImage.style.width = Length.Percent(100);
            iconImage.style.height = Length.Percent(100);

            iconButton.dataSource = character;
            iconButton.Add(iconImage);
            iconButton.AddToClassList("char-icon");

            iconButton.RegisterCallback<ClickEvent>(OnClickIcon);

            _charScrollView.Add(iconButton);
        }
    }

    private void OnClickIcon(ClickEvent evt)
    {
        Button iconButton = evt.currentTarget as Button;
        CharacterBaseSO character = iconButton.dataSource as CharacterBaseSO;
        selectedCharacter = character;
        Logger.Log("Selected: " + selectedCharacter.name);

        UpdateSelectedCharacterUI();
    }

    private void UpdateSelectedCharacterUI()
    {
        _charNameLabel.text = selectedCharacter.name;
        _charSplashArtImage.sprite = selectedCharacter.splashArt;
        _charStatsLabel.text = System.String.Format("HP: {0}\n" +
                                               "Movement Speed: {1}\n" +
                                               "Attack: {2}\n" +
                                               "Attack Speed: {3}\n" +
                                               "Defense: {4}", selectedCharacter.Hp, selectedCharacter.MovementSpeed, 
                                               selectedCharacter.Attack, selectedCharacter.AttackSpeed, selectedCharacter.Defense);
    }
}