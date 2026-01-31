using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ConsoleUIController : MonoBehaviour
{
    public static ConsoleUIController Instance;

    [SerializeField]
    private InputActionAsset _playerActions;

    [SerializeField]
    private InputActionReference _console;

    [SerializeField]
    private InputActionReference _submit;

    private InputActionMap _playerMap;

    private VisualElement _root;
    private VisualElement _consoleElement;
    private ListView _logView;
    private TextField _commandField;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Class setup
        _root = GetComponent<UIDocument>().rootVisualElement;
        _consoleElement = _root.Q<VisualElement>("ConsoleElement");
        _logView = _consoleElement.Q<ListView>("LogView");
        _commandField = _consoleElement.Q<TextField>("CommandField");

        _playerMap = _playerActions.FindActionMap("Player");

        _logView.itemsSource = Logger.Entries;

        _consoleElement.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        if (Instance != this) return;

        Logger.OnLogAdded += UpdateLoggerUI;

        _logView.bindItem += BindLogLabel;
        _logView.makeItem += MakeLogLabel;

        _console.action.Enable();
        _submit.action.Enable();

        _console.action.performed += OpenCloseConsole;
        _submit.action.performed += SubmitCommand;
    }

    private void OnDisable()
    {
        if (Instance != this) return;

        Logger.OnLogAdded -= UpdateLoggerUI;

        _logView.bindItem -= BindLogLabel;
        _logView.makeItem -= MakeLogLabel;

        _console.action.Disable();
        _submit.action.Disable();

        _console.action.performed -= OpenCloseConsole;
        _submit.action.performed -= SubmitCommand;
    }

    private void UpdateLoggerUI(LogEntry log)
    {
        if(_consoleElement.style.display == DisplayStyle.Flex)
        {
            _logView.Rebuild();
            _logView.ScrollToItem(-1);
        }
    }

    private VisualElement MakeLogLabel()
    {
        return new Label();
    }

    private void BindLogLabel(VisualElement element, int index)
    {
        LogEntry entry = Logger.Entries[index];
        Label label = element as Label;

        label.text = System.String.Format("[{0}] - {1}", entry.Timestamp.ToLongTimeString(), entry.Text);
        label.style.color = Color.white;
    }

    private void OpenCloseConsole(InputAction.CallbackContext context)
    {
        if (_consoleElement.style.display == DisplayStyle.None) 
        { 
            _playerMap.Disable();
            _consoleElement.style.display = DisplayStyle.Flex;
            _logView.Rebuild();
            _logView.ScrollToItem(-1);
        } 
        else
        {
            _playerMap.Enable();
            _consoleElement.style.display = DisplayStyle.None;
        }
    }

    private void SubmitCommand(InputAction.CallbackContext context)
    {
        if (_commandField.hasFocusPseudoState && _commandField.text != "")
        {
            Logger.Log(_commandField.text);
            _commandField.SetValueWithoutNotify("");
        }
    }
}