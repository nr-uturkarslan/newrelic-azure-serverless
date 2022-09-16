using System;
namespace DeviceService.Commons.Constants;

public static class EnvironmentVariables
{
    public static string COSMOS_DB_URI { get; set; }

    public static string COSMOS_DB_NAME { get; set; }

    public static string COSMOS_DB_CONTAINER_NAME { get; set; }

    public static string SERVICE_BUS_FQDN { get; set; }

    public static string SERVICE_BUS_QUEUE_NAME { get; set; }
}

