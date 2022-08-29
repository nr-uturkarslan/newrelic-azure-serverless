using System;
using DeviceService.Azure.CosmosDb;
using DeviceService.Controllers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeviceService.Commons.Logging;

public static class CustomLogger
{
    public static void Run(
        ILogger logger,
        CustomLog customLog
    )
    {
        var log = JsonConvert.SerializeObject(
            customLog,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        switch (customLog.LogLevel)
        {
            case LogLevel.Error:
                logger.LogError(log);
                break;

            default:
                logger.LogInformation(log);
                break;
        }
    }
}