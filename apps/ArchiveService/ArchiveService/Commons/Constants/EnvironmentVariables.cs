using System;
namespace ArchiveService.Commons.Constants;

public static class EnvironmentVariables
{
    public static string SERVICE_BUS_FQDN { get; set; }

    public static string SERVICE_BUS_QUEUE_NAME { get; set; }

    public static string STORAGE_ACCOUNT_NAME { get; set; }

    public static string BLOB_CONTAINER_NAME { get; set; }
}

