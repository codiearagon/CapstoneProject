using UnityEngine;

public class ConsoleUIController : MonoBehaviour
{
    private void OnEnable()
    {
        Logger.OnLogAdded += UpdateLoggerUI;
    }

    private void OnDisable()
    {
        Logger.OnLogAdded -= UpdateLoggerUI;
    }

    private void UpdateLoggerUI(LogEntry log)
    {
        Debug.Log(log.Text);
    }
}
