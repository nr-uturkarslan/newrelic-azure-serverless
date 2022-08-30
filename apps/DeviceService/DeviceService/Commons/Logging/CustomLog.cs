using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeviceService.Commons.Logging;

public class CustomLog
{
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

