using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class EventUIController : MonoBehaviour
{
    private Queue<(string, Color)> _eventTitleQueue;

    private VisualElement _root;
    private Label _eventTitle;
    private VisualElement _eventDetailsContainer;
    private bool _titleRunning;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _eventTitleQueue = new Queue<(string, Color)>();

        _eventTitle = _root.Q<Label>("EventTitleLabel");
        _eventDetailsContainer = _root.Q<VisualElement>("EventDetailsContainer");

        _eventTitle.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        GameEvents.OnEvent += HandleEvent;
    }

    private void OnDisable()
    {
        GameEvents.OnEvent -= HandleEvent;
    }

    private void HandleEvent(GameEventMessage message)
    {
        _eventTitleQueue.Enqueue((message.Message, message.Color));
        ShowTitle();

        if(message.Details != null)
        {
            foreach(string detail in message.Details)
            {
                Label label = new Label(detail);
                label.style.color = (Color)new Color32(230, 230, 230, 255);
                label.style.fontSize = 25f;

                StartCoroutine(DestroyLabel(label));
                _eventDetailsContainer.Add(label);
            }
        }
    }

    private IEnumerator DestroyLabel(Label label)
    {
        yield return new WaitForSeconds(3f);
        _eventDetailsContainer.Remove(label);
    }

    private void ShowTitle()
    {
        if (!_titleRunning)
            StartCoroutine(TitleDuration());
    }

    private IEnumerator TitleDuration()
    {
        _titleRunning = true;

        (string text, Color color) = _eventTitleQueue.Dequeue();
        _eventTitle.text = text;
        _eventTitle.style.color = color;

        _eventTitle.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(3f);
        _eventTitle.style.display = DisplayStyle.None;

        if(_eventTitleQueue.Count > 0)
        {
            StartCoroutine(TitleDuration());
            yield break;
        }

        _titleRunning = false;
    }
}
