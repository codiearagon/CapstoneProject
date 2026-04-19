using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CharSelectUIController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _playableCharacters;

    private GameObject _selectedCharacter;

    private VisualElement _root;
    private VisualElement _charSelectElement;

    private Button _backButton;
    private ScrollView _charScrollView;
    private Image _charSplashArtImage;
    private Label _charNameLabel;
    private Label _charStatsLabel;
    private Button _charSelectButton;
    private Image _background;

    private void Awake()
    {
        ResolveReferences();
        SetupScrollView();

        _selectedCharacter = _playableCharacters[0];
        UpdateSelectedCharacterUI();
    }

    private void OnEnable()
    {
        _charSelectButton.RegisterCallback<ClickEvent>(SelectCharacter);
        _backButton.RegisterCallback<ClickEvent>(BackToMenu);
    }

    private void OnDisable()
    {
        _charSelectButton.UnregisterCallback<ClickEvent>(SelectCharacter);
        _backButton.UnregisterCallback<ClickEvent>(BackToMenu);
    }

    private void ResolveReferences()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _charSelectElement = _root.Q<VisualElement>("CharSelectElement");

        _backButton = _charSelectElement.Q<Button>("BackButton");
        _charScrollView = _charSelectElement.Q<ScrollView>("CharScrollView");
        _charSplashArtImage = _charSelectElement.Q<Image>("CharSplashArtImage");
        _charNameLabel = _charSelectElement.Q<Label>("CharNameLabel");
        _charStatsLabel = _charSelectElement.Q<Label>("CharStatsLabel");
        _charSelectButton = _charSelectElement.Q<Button>("CharSelectButton");
        _background = _charSelectElement.Q<Image>("Background");
    }

    private void SetupScrollView()
    {
        foreach(GameObject character in _playableCharacters)
        {
            Button iconButton = new Button();
            Image iconImage = new Image();
            iconImage.sprite = character.GetComponent<CharacterMetadata>().Icon;
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
        GameObject character = iconButton.dataSource as GameObject;

        _selectedCharacter = character;
        Logger.Log("Selected: " + _selectedCharacter.name);

        UpdateSelectedCharacterUI();
    }

    private void UpdateSelectedCharacterUI()
    {
        CharacterMetadata _charMeta = _selectedCharacter.GetComponent<CharacterMetadata>();
        Character _char = _selectedCharacter.GetComponent<Character>();

        _charNameLabel.text = _selectedCharacter.name;
        _charSplashArtImage.sprite = _charMeta.SplashArt;
        _charStatsLabel.text = $"HP: {_char.Stats.MaxHp}\n" +
                               $"Mana: {_char.Stats.MaxMana}\n" +
                               $"Movement Speed: {_char.Stats.MovementSpeed}\n" +
                               $"Attack: {_char.Stats.Attack}\n" +
                               $"Attack Speed: {_char.Stats.AttackSpeed}\n" + 
                               $"Defense: {_char.Stats.Defense}";

        SetBg();
    }
    private void SetBg()
    {
        _background.image = _selectedCharacter.GetComponent<CharacterMetadata>().SplashArt.texture;
        _background.scaleMode = ScaleMode.ScaleAndCrop;
    }

    private void BackToMenu(ClickEvent evt)
    {
        CloseCharSelect();
    }

    public void SelectCharacter(ClickEvent evt)
    {
        PlayerPersistentState.Instance.SetCharacter(_selectedCharacter);
        SceneManager.LoadScene("Main");
    }

    public void OpenCharSelect()
    {
        _charSelectElement.style.display = DisplayStyle.Flex;
    }

    public void CloseCharSelect()
    {
        _charSelectElement.style.display = DisplayStyle.None;
        GetComponent<MainMenuUIController>().OpenMainMenu();
    }
}