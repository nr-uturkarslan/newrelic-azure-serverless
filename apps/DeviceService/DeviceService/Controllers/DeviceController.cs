using System;
using DeviceService.Commons.Logging;
using DeviceService.Services.Create;
using DeviceService.Services.Create.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.Controllers;

[ApiController]
[Route("device")]
public class DeviceController : ControllerBase
{
    private const string CREATE_ENDPOINT_NAME = "Create";

    private readonly ILogger<DeviceController> _logger;

    private readonly ICreateDeviceService _createDeviceService;

    public DeviceController(
        ILogger<DeviceController> logger,
        ICreateDeviceService createDeviceService
    )
    {
        _logger = logger;
        _createDeviceService = createDeviceService;
    }

    [HttpPost(Name = CREATE_ENDPOINT_NAME)]
    [Route("create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateDeviceRequestDto requestDto
    )
    {
        LogEndpointIsTriggered(CREATE_ENDPOINT_NAME);

        var response = await _createDeviceService.Run(requestDto);

        LogEndpointIsFinished(CREATE_ENDPOINT_NAME);

        return new CreatedResult($"{response.Data.Id}", response);
    }

    private void LogEndpointIsTriggered(
        string endpointName
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(DeviceController),
                MethodName = nameof(Create),
                LogLevel = LogLevel.Information,
                Message = $"{endpointName} endpoint is triggered...",
            });
    }

    private void LogEndpointIsFinished(
        string endpointName
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(DeviceController),
                MethodName = nameof(Create),
                LogLevel = LogLevel.Information,
                Message = $"{endpointName} endpoint is finished.",
            });
    }
}
