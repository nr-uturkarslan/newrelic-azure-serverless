using System;
using System.Reflection;
using Azure.Identity;
using DeviceService.Commons.Constants;
using DeviceService.Commons.Logging;
using DeviceService.Entities;
using Microsoft.Azure.Cosmos;

namespace DeviceService.Azure.CosmosDb;

public interface ICosmosDbHandler
{
    Task CreateItem(Device device);
}

public class CosmosDbHandler : ICosmosDbHandler
{
    private readonly ILogger<CosmosDbHandler> _logger;
    private readonly Container _container;

    public CosmosDbHandler(
        ILogger<CosmosDbHandler> logger
    )
    {
        _logger = logger;

        LogStartingCosmosDbClient();

        try
        {
            var cosmosClient = new CosmosClient(
                EnvironmentVariables.COSMOS_DB_URI,
                new DefaultAzureCredential()
            );

            _container = cosmosClient.GetContainer(
                EnvironmentVariables.COSMOS_DB_NAME,
                EnvironmentVariables.COSMOS_DB_CONTAINER_NAME
            );

            LogCosmosDbClientStartedSuccessfully();
        }
        catch (Exception e)
        {
            LogCosmosDbClientStartingFailed(e);
        }
    }

    public async Task CreateItem(
        Device device
    )
    {
        await _container.CreateItemAsync<Device>(
            device,
            new PartitionKey(device.Id)
        );
    }

    private void LogStartingCosmosDbClient()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(CosmosDbHandler),
                MethodName = nameof(CosmosDbHandler),
                LogLevel = LogLevel.Information,
                Message = $"Starting Cosmos DB client...",
            });
    }

    private void LogCosmosDbClientStartingFailed(
        Exception e
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(CosmosDbHandler),
                MethodName = nameof(CosmosDbHandler),
                LogLevel = LogLevel.Error,
                Message = $"Starting Cosmos DB client is failed.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }

    private void LogCosmosDbClientStartedSuccessfully()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(CosmosDbHandler),
                MethodName = nameof(CosmosDbHandler),
                LogLevel = LogLevel.Information,
                Message = $"Cosmos DB client is started successfully.",
            });
    }
}

