using System;
using ArchiveService.Commons.Constants;
using ArchiveService.Commons.Logging;
using ArchiveService.Entities;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace ArchiveService.Azure.ServiceBus;

public class ServiceBusHandler : IHostedService
{
    private readonly ILogger _logger;

    private ServiceBusClient _client;
    private ServiceBusProcessor _processor;

    public ServiceBusHandler(
        ILogger<ServiceBusHandler> logger
    )
    {
        // Set logger.
        _logger = logger;


        // Create Service Bus processor.
        CreateServiceBusProcessor();
    }

    private void CreateServiceBusProcessor()
    {
        LogCreatingServiceBusProcessor();

        _client = new ServiceBusClient(
            EnvironmentVariables.SERVICE_BUS_FQDN,
            new DefaultAzureCredential()
        );

        var serviceBusProcessorOptions = new ServiceBusProcessorOptions
        {
            ReceiveMode = ServiceBusReceiveMode.PeekLock
        };

        _processor = _client.CreateProcessor(
            EnvironmentVariables.SERVICE_BUS_QUEUE_NAME,
            serviceBusProcessorOptions
        );

        // Add handler to process messages
        _processor.ProcessMessageAsync += MessageHandler;

        // Add handler to process any errors
        _processor.ProcessErrorAsync += ErrorHandler;

        LogServiceBusProcessorCreated();
    }

    public async Task StartAsync(
        CancellationToken cancellationToken
    )
    {
        try
        {
            LogStartingServiceBusProcessor();

            await _processor.StartProcessingAsync();
        }
        catch (Exception e)
        {
            LogUnexpectedErrorOccured(e);

            await _processor.DisposeAsync();
            await _client.DisposeAsync();
        }
    }

    public async Task StopAsync(
        CancellationToken cancellationToken
    )
    {
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }

    private async Task MessageHandler(
        ProcessMessageEventArgs args
    )
    {
        try
        {
            // Parse the message.
            var deviceMessage = ParseMessage(args.Message);

            // Acknowledge the message.
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception e)
        {
            LogUnexpectedErrorOccured(e);
        }
    }

    private Device ParseMessage(
        ServiceBusReceivedMessage message
    )
    {
        LogParsingServiceBusMessage();

        var deviceMessage = JsonConvert.DeserializeObject<Device>(
            message.Body.ToString());

        LogServiceBusMessageParsed();

        return deviceMessage;
    }

    private Task ErrorHandler(
        ProcessErrorEventArgs args
    )
    {
        LogServiceBusErrorOccured(args.Exception);
        return Task.CompletedTask;
    }

    private void LogCreatingServiceBusProcessor()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(CreateServiceBusProcessor),
                LogLevel = LogLevel.Information,
                Message = "Creating Service Bus processor...",
            });
    }

    private void LogServiceBusProcessorCreated()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(CreateServiceBusProcessor),
                LogLevel = LogLevel.Information,
                Message = "Service Bus processor is created successfully.",
            });
    }

    private void LogStartingServiceBusProcessor()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(StartAsync),
                LogLevel = LogLevel.Information,
                Message = "Starting Service Bus processor...",
            });
    }

    private void LogParsingServiceBusMessage()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(ParseMessage),
                LogLevel = LogLevel.Information,
                Message = "Parsing Service Bus message...",
            });
    }

    private void LogServiceBusMessageParsed()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(ParseMessage),
                LogLevel = LogLevel.Information,
                Message = "Service Bus message parsed.",
            });
    }

    private void LogUnexpectedErrorOccured(
        Exception e
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(MessageHandler),
                LogLevel = LogLevel.Error,
                Message = "Unexpected error occurred.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }

    private void LogServiceBusErrorOccured(
        Exception e
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(ErrorHandler),
                LogLevel = LogLevel.Error,
                Message = "Service Bus error occurred.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }
}

