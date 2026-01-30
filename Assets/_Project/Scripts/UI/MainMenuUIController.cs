using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUIController : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _mainMenuElement;

    private Button _startButton;
    private Button _settingsButton;
    private Button _quitButton;

    private void Awake()
    {
        ResolveReferences(); 
    }

    private void OnEnable()
    {
        _startButton.RegisterCallback<ClickEvent>(OnClickStart);
        _settingsButton.RegisterCallback<ClickEvent>(OnClickSettings);
        _quitButton.RegisterCallback<ClickEvent>(OnClickQuit);
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnClickStart);
        _settingsButton.UnregisterCallback<ClickEvent>(OnClickSettings);
        _quitButton.UnregisterCallback<ClickEvent>(OnClickQuit);
    }

    public void OpenMainMenu()
    {
        _mainMenuElement.style.display = DisplayStyle.Flex;
    }

    public void CloseMainMenu()
    {
        _mainMenuElement.style.display = DisplayStyle.None;
    }

    private void OnClickStart(ClickEvent evt)
    {
        Logger.Log("Clicked Start");
        GetComponent<CharSelectUIController>().OpenCharSelect();
        CloseMainMenu();
    }

    private void OnClickSettings(ClickEvent evt)
    {
        Logger.Log("Clicked Settings");
    }

    private void OnClickQuit(ClickEvent evt)
    {
        Logger.Log("Clicked Quit");
    }

    private void ResolveReferences()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _mainMenuElement = _root.Q<VisualElement>("MainMenuElement");

        _startButton = _mainMenuElement.Q<Button>("StartButton");
        _settingsButton = _mainMenuElement.Q<Button>("SettingsButton");
        _quitButton = _mainMenuElement.Q<Button>("QuitButton");
    }
}
