using System;
using UnityEngine;

public enum LogType
{
    Normal,
    Error,
    Warning
}

public class LogEntry
{
    public LogType Type { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string Text { get; private set; }

    public LogEntry(object text, LogType type = LogType.Normal)
    {
        Type = type;
        Timestamp = DateTime.Now;
        Text = text.ToString();
    }
}
