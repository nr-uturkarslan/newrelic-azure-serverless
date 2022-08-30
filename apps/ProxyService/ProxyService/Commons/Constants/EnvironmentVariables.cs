using System;
namespace ProxyService.Commons.Constants;

public static class EnvironmentVariables
{
    public static string NEW_RELIC_APP_NAME { get; set; }

    public static string NEW_RELIC_LICENSE_KEY { get; set; }

    public static string NEW_RELIC_OTEL_EXPORTER_OTLP_ENDPOINT { get; set; }
    
    public static string DEVICE_SERVICE_URI_CREATE { get; set; }
}

