using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DeathUIController : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _container;

    private Button _restartButton;
    private Button _menuButton;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        
        _container = _root.Q<VisualElement>("Container");
        _restartButton = _root.Q<Button>("Restart");
        _menuButton = _root.Q<Button>("Menu");

        _container.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _restartButton.RegisterCallback<ClickEvent>(Restart);
        _menuButton.RegisterCallback<ClickEvent>(BackToMenu);
    }

    private void OnDisable()
    {
        _restartButton.UnregisterCallback<ClickEvent>(Restart);
        _menuButton.UnregisterCallback<ClickEvent>(BackToMenu);
    }

    private void Restart(ClickEvent evt)
    {
        Utility.ReleasePause();
        SceneManager.LoadScene("Main");
    }

    private void BackToMenu(ClickEvent evt)
    {
        Utility.ReleasePause();
        SceneManager.LoadScene("Menu");
    }

    public void TriggerUI()
    {
        _container.style.display = DisplayStyle.Flex;
    }

}
