using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ProxyService.Services.Device.Create;
using ProxyService.Commons.Constants;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

[assembly: FunctionsStartup(typeof(ProxyService.Startup))]

namespace ProxyService;

public class Startup : FunctionsStartup
{
    public override void Configure(
        IFunctionsHostBuilder builder
    )
    {
        GetEnvironmentVariables();

        builder.Services.AddHttpClient();

        // shared Resource to use for both OTel metrics AND tracing
        var resource = ResourceBuilder.CreateDefault()
            .AddService(EnvironmentVariables.NEW_RELIC_APP_NAME);

        builder.Services.AddOpenTelemetryTracing(b =>
        {
            // use the OTLP exporter
            b.AddOtlpExporter();

            // receive traces from our own custom sources
            b.AddSource(EnvironmentVariables.NEW_RELIC_APP_NAME);

            // decorate our service name so we can find it when we search traces
            b.SetResourceBuilder(resource);

            // receive traces from built-in sources
            b.AddHttpClientInstrumentation();
            b.AddAspNetCoreInstrumentation();
        });

        builder.Services.AddOpenTelemetryMetrics(b =>
        {
            // add prometheus exporter
            b.AddOtlpExporter();

            // receive metrics from our own custom sources
            b.AddMeter(EnvironmentVariables.NEW_RELIC_APP_NAME);

            // decorate our service name so we can find it when we search metrics
            b.SetResourceBuilder(resource);

            // receive metrics from built-in sources
            b.AddHttpClientInstrumentation();
            b.AddAspNetCoreInstrumentation();
        });

        builder.Services.AddSingleton<ICreateDeviceService, CreateDeviceService>();
        
    }

    private void GetEnvironmentVariables()
    {
        Console.WriteLine("Getting environment variables...");

        var newRelicAppName = Environment.GetEnvironmentVariable("NEW_RELIC_APP_NAME");
        if (string.IsNullOrEmpty(newRelicAppName))
        {
            Console.WriteLine("[NEW_RELIC_APP_NAME] is not provided");
            Environment.Exit(1);
        }
        EnvironmentVariables.NEW_RELIC_APP_NAME = newRelicAppName;

        var deviceServiceUri = Environment.GetEnvironmentVariable("DEVICE_SERVICE_URI");
        if (string.IsNullOrEmpty(deviceServiceUri))
        {
            Console.WriteLine("[DEVICE_SERVICE_URI] is not provided");
            Environment.Exit(1);
        }
        EnvironmentVariables.DEVICE_SERVICE_URI_CREATE = deviceServiceUri + "/device/create";
    }
}
