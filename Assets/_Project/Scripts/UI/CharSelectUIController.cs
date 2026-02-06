using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharSelectUIController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _playableCharacters;

    private GameObject _selectedCharacter;

    private VisualElement _root;
    private VisualElement _charSelectElement;

    private ScrollView _charScrollView;
    private Image _charSplashArtImage;
    private Label _charNameLabel;
    private Label _charStatsLabel;
    private Button _charSelectButton;

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
    }

    private void OnDisable()
    {
        _charSelectButton.UnregisterCallback<ClickEvent>(SelectCharacter);
    }

    private void ResolveReferences()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _charSelectElement = _root.Q<VisualElement>("CharSelectElement");

        _charScrollView = _charSelectElement.Q<ScrollView>("CharScrollView");
        _charSplashArtImage = _charSelectElement.Q<Image>("CharSplashArtImage");
        _charNameLabel = _charSelectElement.Q<Label>("CharNameLabel");
        _charStatsLabel = _charSelectElement.Q<Label>("CharStatsLabel");
        _charSelectButton = _charSelectElement.Q<Button>("CharSelectButton");
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

        _charNameLabel.text = _selectedCharacter.name;
        _charSplashArtImage.sprite = _charMeta.Icon;
        _charStatsLabel.text = System.String.Format("HP: {0}\n" +
                                               "Movement Speed: {1}\n" +
                                               "Attack: {2}\n" +
                                               "Attack Speed: {3}\n" +
                                               "Defense: {4}", _charMeta.Stats.MaxHp, _charMeta.Stats.MovementSpeed,
                                               _charMeta.Stats.Attack, _charMeta.Stats.AttackSpeed, _charMeta.Stats.Defense);
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
    }
}