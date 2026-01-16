using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
    [SerializeField]
    private VoidEventSO _voidEvent;

    [SerializeField]
    private UnityEvent _response;

    private void OnEnable()
    {
        _voidEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        _voidEvent?.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        _response.Invoke();
    }
}
