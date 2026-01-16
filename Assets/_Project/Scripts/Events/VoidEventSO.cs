using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu (fileName = "New Void Event", menuName = "Events/New Void Event")]
public class VoidEventSO : ScriptableObject
{
    private List<VoidEventListener> _listeners = new List<VoidEventListener>();

    public void RegisterListener(VoidEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(VoidEventListener listener)
    {
        _listeners.Remove(listener);
    }

    public void RaiseEvent()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }
}
