using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu (fileName = "New Void Event", menuName = "Events/New Void Event")]
public class VoidEventSO : ScriptableObject
{
    private List<VoidEventListener> listeners = new List<VoidEventListener>();

    public void RegisterListener(VoidEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(VoidEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void RaiseEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }
}
