using System;
using System.Net;
using DeviceService.Azure.CosmosDb;
using DeviceService.Commons.Logging;
using DeviceService.Controllers;
using DeviceService.Dtos;
using DeviceService.Entities;
using DeviceService.Services.Create.Dtos;
using Microsoft.Extensions.Logging;

namespace DeviceService.Services.Create;

public interface ICreateDeviceService
{
    Task<ResponseTemplate<CreateDeviceResponseDto>> Run(
        CreateDeviceRequestDto requestDto
    );
}

public class CreateDeviceService : ICreateDeviceService
{
    private readonly ILogger<ICreateDeviceService> _logger;

    private readonly ICosmosDbHandler _cosmosDbHandler;

    public CreateDeviceService(
        ILogger<ICreateDeviceService> logger,
        ICosmosDbHandler cosmosDbHandler
    )
    {
        _logger = logger;
        _cosmosDbHandler = cosmosDbHandler;
    }

    public async Task<ResponseTemplate<CreateDeviceResponseDto>> Run(
        CreateDeviceRequestDto requestDto
    )
    {
        try
        {
            LogCreatingDevice();

            var device = new Device
            {
                Id = Guid.NewGuid().ToString(),
                Name = requestDto.Name,
                Description = requestDto.Description,
                isValid = false,
            };

            await _cosmosDbHandler.CreateItem(device);

            var responseDto = new CreateDeviceResponseDto
            {
                Id = device.Id,
                Name = device.Name,
                Description = device.Description,
            };

            var response = new ResponseTemplate<CreateDeviceResponseDto>
            {
                Message = "Device is created successfully.",
                StatusCode = HttpStatusCode.Created,
                Data = responseDto,
            };

            LogDeviceCreatedSuccessfully();

            return response;
        }
        catch (Exception e)
        {
            LogDeviceCreationFailed(e);
            var response = new ResponseTemplate<CreateDeviceResponseDto>
            {
                Message = "Device creation is failed.",
                StatusCode = HttpStatusCode.InternalServerError, 
            };

            return response;
        }
    }

    private void LogCreatingDevice()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(DeviceController),
                MethodName = nameof(Run),
                LogLevel = LogLevel.Information,
                Message = "Creating device...",
            });
    }

    private void LogDeviceCreatedSuccessfully()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(DeviceController),
                MethodName = nameof(Run),
                LogLevel = LogLevel.Information,
                Message = "Device is created successfully.",
            });
    }

    private void LogDeviceCreationFailed(
        Exception e
    )
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(DeviceController),
                MethodName = nameof(Run),
                LogLevel = LogLevel.Error,
                Message = "Device creation is failed.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }
}

