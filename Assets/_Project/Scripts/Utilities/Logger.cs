using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class Logger
{
    public static event Action<LogEntry> OnLogAdded;

    private static List<LogEntry> _entries = new List<LogEntry>();
    public static ReadOnlyCollection<LogEntry> Entries => _entries.AsReadOnly();

    public static void Log(object text, LogType type = LogType.Normal)
    {
        LogEntry entry = new LogEntry(text, type);
        _entries.Add(entry);
        OnLogAdded?.Invoke(entry);
    }
}
