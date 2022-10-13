using System;
using System.Reflection.Metadata;
using ArchiveService.Azure.BlobContainer;
using ArchiveService.Commons.Constants;
using ArchiveService.Commons.Exceptions;
using ArchiveService.Commons.Logging;
using ArchiveService.Entities;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using NewRelic.Api.Agent;
using Newtonsoft.Json;

namespace ArchiveService.Azure.ServiceBus;

public class ServiceBusHandler : IHostedService
{
    private readonly ILogger _logger;

    private readonly IBlobHandler _blobHandler;

    private ServiceBusClient _client;
    private ServiceBusProcessor _processor;

    public ServiceBusHandler(
        ILogger<ServiceBusHandler> logger,
        IBlobHandler blobHandler
    )
    {
        // Set logger.
        _logger = logger;

        // Set blob handler.
        _blobHandler = blobHandler;

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
            // Get distributed tracing headers.
            AddDistributedTracingHeadersIfGiven(args.Message);

            // Parse the message.
            var deviceMessage = ParseMessage(args.Message);

            // Store the message in blob container.
            await StoreMessageInBlobContainer(deviceMessage);

            // Acknowledge the message.
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception e)
        {
            LogUnexpectedErrorOccured(e);
        }
    }

    private void AddDistributedTracingHeadersIfGiven(
        ServiceBusReceivedMessage message
    )
    {
        var transaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
        transaction.AcceptDistributedTraceHeaders(message, Getter, TransportType.Queue);
    }

    private IEnumerable<string> Getter(
        ServiceBusReceivedMessage carrier,
        string headerKey
    )
    {
        if (headerKey.Equals("traceparent") || headerKey.Equals("tracestate"))
        {
            object headerValue;
            var found = carrier.ApplicationProperties
                .TryGetValue(headerKey, out headerValue);

            string value = null;
            if (found)
            {
                value = headerValue.ToString();
                LogAddingDistributedTraceHeader(headerKey, value);
            }

            return value == null ? null : new string[] { value };
        }
        else
            return null;
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

    private async Task StoreMessageInBlobContainer(
        Device deviceMessage
    )
        => await _blobHandler.Upload(deviceMessage);

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

    private void LogAddingDistributedTraceHeader(
        string headerKey,
        string headerValue
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(AddDistributedTracingHeadersIfGiven),
                LogLevel = LogLevel.Information,
                Message = $"Adding distributed trace headers [{headerKey} -> {headerValue}]",
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

