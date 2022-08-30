using System;
using Newtonsoft.Json;

namespace ArchiveService.Commons.Logging;

public class CustomLog
{
    // Code related properties
    [JsonProperty("className")]
    public string ClassName { get; set; }

    [JsonProperty("methodName")]
    public string MethodName { get; set; }

    [JsonProperty("logLevel")]
    public LogLevel LogLevel { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("exception")]
    public string Exception { get; set; }

    [JsonProperty("stackTrace")]
    public string StackTrace { get; set; }
}

