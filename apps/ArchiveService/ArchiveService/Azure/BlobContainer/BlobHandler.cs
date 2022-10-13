using System;
using System.Reflection.Metadata;
using ArchiveService.Azure.ServiceBus;
using ArchiveService.Commons.Constants;
using ArchiveService.Commons.Exceptions;
using ArchiveService.Commons.Logging;
using ArchiveService.Entities;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Newtonsoft.Json;

namespace ArchiveService.Azure.BlobContainer;

public interface IBlobHandler
{
    Task Upload(
        Device device
    );
}

public class BlobHandler : IBlobHandler
{
    private readonly ILogger<BlobHandler> _logger;

    private BlobContainerClient _blobContainerClient;

    public BlobHandler(
        ILogger<BlobHandler> logger
    )
    {
        // Set logger.
        _logger = logger;

        // Create Service Bus processor.
        CreateBlobContainerClient();
    }

    private void CreateBlobContainerClient()
    {
        LogCreatingBlobContainerClient();

        _blobContainerClient = new BlobContainerClient(
            new Uri($"https://{EnvironmentVariables.STORAGE_ACCOUNT_NAME}.blob.core.windows.net/{EnvironmentVariables.BLOB_CONTAINER_NAME}"),
            new DefaultAzureCredential(),
            new BlobClientOptions()
        );

        LogCreatingBlobContainerClientSucceeded();
    }

    public async Task Upload(
        Device device
    )
    {
        try
        {
            LogStoringMessageInBlobContainer();

            var deviceMessageAsString = JsonConvert.SerializeObject(device);
            var blobClient = _blobContainerClient.GetBlobClient(device.Id);

            await blobClient.UploadAsync(
                BinaryData.FromString(deviceMessageAsString),
                overwrite: true
            );

            LogStoringMessageInBlobContainerSucceeded();
        }
        catch (Exception e)
        {
            LogStoringMessageInBlobContainerFailed(e);
            throw new BlobUploadFailedException();
        }
    }

    private void LogCreatingBlobContainerClient()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(BlobHandler),
                MethodName = nameof(CreateBlobContainerClient),
                LogLevel = LogLevel.Information,
                Message = "Creating Blob container client...",
            });
    }

    private void LogCreatingBlobContainerClientSucceeded()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(BlobHandler),
                MethodName = nameof(CreateBlobContainerClient),
                LogLevel = LogLevel.Information,
                Message = "Creating Blob container client is succeeded.",
            });
    }

    private void LogStoringMessageInBlobContainer()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(BlobHandler),
                MethodName = nameof(Upload),
                LogLevel = LogLevel.Information,
                Message = "Storing device message in blob container...",
            });
    }

    private void LogStoringMessageInBlobContainerSucceeded()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(BlobHandler),
                MethodName = nameof(Upload),
                LogLevel = LogLevel.Information,
                Message = "Storing device message in blob container is succeeded.",
            });
    }

    private void LogStoringMessageInBlobContainerFailed(
        Exception e
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(BlobHandler),
                MethodName = nameof(Upload),
                LogLevel = LogLevel.Error,
                Message = "Storing device message in blob container is failed.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }
}

