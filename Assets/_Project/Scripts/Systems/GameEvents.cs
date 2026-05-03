using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventType
{
    LevelUp,
    EliteSpawned,
    BossSpawned,
    BossPhaseChanged,
    EnemiesStronger
}

public class GameEventMessage
{
    public GameEventType Type;
    public string Message;
    public Color Color;
    public List<string> Details;

    public GameEventMessage(GameEventType type, string message, Color color, List<string> details = null)
    {
        Type = type;
        Message = message;
        Color = color;
        Details = details;
    }
}

public class GameEvents : MonoBehaviour
{
    public static event Action<GameEventMessage> OnEvent;

    public static void Raise(GameEventMessage message)
    {
        OnEvent?.Invoke(message);
    }
}
