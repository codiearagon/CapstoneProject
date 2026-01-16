using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
    [SerializeField]
    private VoidEventSO voidEvent;

    [SerializeField]
    private UnityEvent response;

    private void OnEnable()
    {
        voidEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        voidEvent?.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}
