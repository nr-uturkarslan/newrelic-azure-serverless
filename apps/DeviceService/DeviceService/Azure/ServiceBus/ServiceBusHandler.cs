using System;
using System.Net;
using System.Text;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using DeviceService.Commons.Constants;
using DeviceService.Commons.Logging;
using DeviceService.Services.Create.Dtos;
using Microsoft.Azure.Cosmos;
using NewRelic.Api.Agent;
using Newtonsoft.Json;

namespace DeviceService.Azure.ServiceBus;

public interface IServiceBusHandler
{
    Task Publish(
        CreateDeviceRequestDto requestDto
    );
}

public class ServiceBusHandler : IServiceBusHandler
{
    private readonly ILogger<ServiceBusHandler> _logger;
    private readonly ServiceBusSender _sender;

    public ServiceBusHandler(
        ILogger<ServiceBusHandler> logger
    )
    {
        _logger = logger;

        try
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            var client = new ServiceBusClient(
                EnvironmentVariables.SERVICE_BUS_FQDN,
                new DefaultAzureCredential()
            );

            _sender = client.CreateSender(
                EnvironmentVariables.SERVICE_BUS_QUEUE_NAME
            );
        }
        catch (Exception e)
        {
            LogServiceBusClientNotCreated(e);
            Environment.Exit(1);
        }
        
    }

    public async Task Publish(
        CreateDeviceRequestDto requestDto
    )
    {
        try
        {
            LogPublishingMessage();

            var message = new ServiceBusMessage(
                JsonConvert.SerializeObject(requestDto));

            AddDistributedTracingHeadersIfExist(message);

            await _sender.SendMessageAsync(message);

            LogMessagePublished();
        }
        catch (Exception e)
        {
            LogMessageNotPublished(e);
        }
    }

    private void AddDistributedTracingHeadersIfExist(
        ServiceBusMessage message
    )
    {
        var distributedTracingHeaders = new Dictionary<String, String>();
        var transaction = NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;

        var setter = new Action<Dictionary<String, String>, string, string>(
            (carrier, key, value) => {
                if (key.Equals("traceparent") || key.Equals("tracestate"))
                carrier.Add(key, value);
            });
        transaction.InsertDistributedTraceHeaders(distributedTracingHeaders, setter);

        foreach (var header in distributedTracingHeaders)
        {
            LogAddingDistributedTraceHeader(header.Key, header.Value);
            message.ApplicationProperties.Add(header.Key , header.Value);
        }
    }

    private void LogCreatingServiceBusClient()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(ServiceBusHandler),
                LogLevel = LogLevel.Information,
                Message = "Creating Service Bus client...",
            });
    }

    private void LogServiceBusClientCreated()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(ServiceBusHandler),
                LogLevel = LogLevel.Information,
                Message = "Service Bus client is created.",
            });
    }

    private void LogServiceBusClientNotCreated(
        Exception e
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(ServiceBusHandler),
                LogLevel = LogLevel.Error,
                Message = "Service Bus client is not created.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }

    private void LogPublishingMessage()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(Publish),
                LogLevel = LogLevel.Information,
                Message = "Publishing message...",
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
                MethodName = nameof(AddDistributedTracingHeadersIfExist),
                LogLevel = LogLevel.Information,
                Message = $"Adding distributed trace headers [{headerKey} -> {headerValue}]",
            });
    }

    private void LogMessagePublished()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(Publish),
                LogLevel = LogLevel.Information,
                Message = "Message is published.",
            });
    }

    private void LogMessageNotPublished(
        Exception e
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ServiceBusHandler),
                MethodName = nameof(ServiceBusHandler),
                LogLevel = LogLevel.Error,
                Message = "Message is not published.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }
}

