using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ProxyService.Services.Device.Create;
using ProxyService.Commons.Constants;

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

        builder.Services.AddSingleton<ICreateDeviceService, CreateDeviceService>();
    }

    private void GetEnvironmentVariables()
    {
        Console.WriteLine("Getting environment variables...");

        var deviceServiceUri = Environment.GetEnvironmentVariable("DEVICE_SERVICE_URI");
        if (string.IsNullOrEmpty(deviceServiceUri))
        {
            Console.WriteLine("[DEVICE_SERVICE_URI] is not provided");
            Environment.Exit(1);
        }
        EnvironmentVariables.DEVICE_SERVICE_URI_CREATE = deviceServiceUri + "/device/create";
    }
}
