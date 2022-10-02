using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ProxyService.Services.Device.Create;
using ProxyService.Commons.Constants;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections.Generic;

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
        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(EnvironmentVariables.NEW_RELIC_APP_NAME)
            //.AddAttributes(new Dictionary<string, object> {
            //    { "environment", "production" }
            //})
            .AddTelemetrySdk();

        builder.Services.AddOpenTelemetryTracing(b =>
        {
            // decorate our service name so we can find it when we search traces
            b.SetResourceBuilder(resourceBuilder);

            // receive traces from built-in sources
            b.AddHttpClientInstrumentation();
            b.AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
            });

            // use the OTLP exporter
            b.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri($"{EnvironmentVariables.NEW_RELIC_OTEL_EXPORTER_OTLP_ENDPOINT}");
                options.Headers = $"api-key={EnvironmentVariables.NEW_RELIC_LICENSE_KEY}";
            });

            // receive traces from our own custom sources
            b.AddSource(EnvironmentVariables.NEW_RELIC_APP_NAME);
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

        var newRelicLicenseKey = Environment.GetEnvironmentVariable("NEW_RELIC_LICENSE_KEY");
        if (string.IsNullOrEmpty(newRelicLicenseKey))
        {
            Console.WriteLine("[NEW_RELIC_LICENSE_KEY] is not provided");
            Environment.Exit(1);
        }
        EnvironmentVariables.NEW_RELIC_LICENSE_KEY = newRelicLicenseKey;

        var newRelicOtelExporterOtlpEndpoint = Environment.GetEnvironmentVariable("NEW_RELIC_OTEL_EXPORTER_OTLP_ENDPOINT");
        if (string.IsNullOrEmpty(newRelicOtelExporterOtlpEndpoint))
        {
            Console.WriteLine("[NEW_RELIC_OTEL_EXPORTER_OTLP_ENDPOINT] is not provided");
            Environment.Exit(1);
        }
        EnvironmentVariables.NEW_RELIC_OTEL_EXPORTER_OTLP_ENDPOINT = newRelicOtelExporterOtlpEndpoint;

        var deviceServiceUri = Environment.GetEnvironmentVariable("DEVICE_SERVICE_URI");
        if (string.IsNullOrEmpty(deviceServiceUri))
        {
            Console.WriteLine("[DEVICE_SERVICE_URI] is not provided");
            Environment.Exit(1);
        }
        EnvironmentVariables.DEVICE_SERVICE_URI_CREATE = deviceServiceUri + "/device/create";
    }
}
