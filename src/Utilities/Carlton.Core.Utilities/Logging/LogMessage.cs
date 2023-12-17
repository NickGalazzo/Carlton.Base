﻿namespace Carlton.Core.Utilities.Logging;

public class LogMessage
{
    public LogLevel LogLevel { get; set; }
    public int EventId { get; set; }
    public string EventName { get; set; }
    public string Message { get; set; }
    public Exception Exception { get; set; }
    public DateTime Timestamp { get; set; }
    public string Scopes { get; set; }
}