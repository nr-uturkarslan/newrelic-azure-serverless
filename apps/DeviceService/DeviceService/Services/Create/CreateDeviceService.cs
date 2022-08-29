using System;
using System.Net;
using DeviceService.Azure.CosmosDb;
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

        return response;
    }
}

