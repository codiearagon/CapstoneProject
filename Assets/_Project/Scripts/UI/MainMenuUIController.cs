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

        _startButton.RegisterCallback<ClickEvent>(OnClickStart);
        _settingsButton.RegisterCallback<ClickEvent>(OnClickSettings);
        _quitButton.RegisterCallback<ClickEvent>(OnClickQuit);
    }

    private void OnClickStart(ClickEvent evt)
    {
        Logger.Log("Clicked Start");
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
