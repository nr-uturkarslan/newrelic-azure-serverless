using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using ProxyService.Services.Device.Create;
using ProxyService.Commons.Logging;
using System.Net;

namespace ProxyService
{
    public class ProxyService
    {
        private const string CREATE_DEVICE_ENDPOINT = "CreateDevice";

        private readonly ICreateDeviceService _createDeviceService;

        public ProxyService(
            ICreateDeviceService createDeviceService
        )
        {
            _createDeviceService = createDeviceService;
        }

        [FunctionName(CREATE_DEVICE_ENDPOINT)]
        public async Task<IActionResult> CreateDevice(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "device/create"
            )] HttpRequest req,
            ILogger logger)
        {
            LogEndpointIsTriggered(logger, CREATE_DEVICE_ENDPOINT);

            var response = await _createDeviceService.Run(logger, req);

            LogEndpointIsFinished(logger, CREATE_DEVICE_ENDPOINT);

            var result = new ObjectResult(response);
            result.StatusCode = (int)response.StatusCode;
            return result;
        }

        private void LogEndpointIsTriggered(
            ILogger logger,
            string endpointName
        )
        {
            CustomLogger.Run(logger,
                new CustomLog
                {
                    ClassName = nameof(ProxyService),
                    MethodName = nameof(CreateDevice),
                    LogLevel = LogLevel.Information,
                    Message = $"{endpointName} endpoint is triggered...",
                });
        }

        private void LogEndpointIsFinished(
            ILogger logger,
            string endpointName
        )
        {
            CustomLogger.Run(logger,
                new CustomLog
                {
                    ClassName = nameof(ProxyService),
                    MethodName = nameof(CreateDevice),
                    LogLevel = LogLevel.Information,
                    Message = $"{endpointName} endpoint is finished.",
                });
        }
    }
}

