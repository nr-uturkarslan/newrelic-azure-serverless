using System;
using ProxyService.Services.Device.Create.Dtos;
using System.Threading.Tasks;
using ProxyService.Dtos;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProxyService.Commons.Logging;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using ProxyService.Commons.Exceptions;
using System.Net;
using ProxyService.Commons.Constants;

namespace ProxyService.Services.Device.Create;

public interface ICreateDeviceService
{
    Task<ResponseTemplate<CreateDeviceResponseDto>> Run(
        ILogger logger,
        HttpRequest req
    );
}

public class CreateDeviceService : ICreateDeviceService
{
    private readonly HttpClient _httpClient;

    public CreateDeviceService(
        HttpClient httpClient
    )
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseTemplate<CreateDeviceResponseDto>> Run(
        ILogger logger,
        HttpRequest req
    )
    {
        try
        {
            var requestDto = await ParseRequestBody(logger, req);

            return await ForwardRequestToDeviceService(logger, requestDto);
        }
        catch (RequestBodyNotParsedException)
        {
            return new ResponseTemplate<CreateDeviceResponseDto>
            {
                Message = "Request body could not be parsed.",
                StatusCode = HttpStatusCode.BadRequest,
            };
        }
        catch (Exception e)
        {
            LogUnexpectedErrorOccurred(logger, e);
            return new ResponseTemplate<CreateDeviceResponseDto>
            {
                Message = "Unexcepted error occurred.",
                StatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }

    private async Task<CreateDeviceRequestDto> ParseRequestBody(
        ILogger logger,
        HttpRequest req
    )
    {
        LogParsingRequestBody(logger);

        try
        {
            using (var reader = new StreamReader(req.Body, Encoding.UTF8))
            {
                var requestDto = JsonConvert.DeserializeObject<
                    CreateDeviceRequestDto>(
                        await reader.ReadToEndAsync()
                    );

                LogRequestBodyParsingSucceeded(logger);
                return requestDto;
            }
        }
        catch (Exception e)
        {
            LogRequestBodyParsingFailed(logger, e);
            throw new RequestBodyNotParsedException();
        }
    }

    private async Task<ResponseTemplate<CreateDeviceResponseDto>> ForwardRequestToDeviceService(
        ILogger logger,
        CreateDeviceRequestDto requestDto
    )
    {
        LogForwardingRequestToDeviceService(logger);

        var requestDtoAsString = JsonConvert.SerializeObject(requestDto);

        var stringContent = new StringContent(
            requestDtoAsString,
            Encoding.UTF8,
            "application/json"
        );

        var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            EnvironmentVariables.DEVICE_SERVICE_URI_CREATE
        )
        {
            Content = stringContent
        };

        var response = await _httpClient.SendAsync(httpRequest);
        var responseDtoAsString = await response.Content.ReadAsStringAsync();

        var responseDto = JsonConvert.DeserializeObject<
            ResponseTemplate<CreateDeviceResponseDto>>(responseDtoAsString);

        LogForwardingRequestToDeviceServiceSucceeded(logger);

        return responseDto;
    }

    private void LogParsingRequestBody(
        ILogger logger
    )
    {
        CustomLogger.Run(logger,
            new CustomLog
            {
                ClassName = nameof(CreateDeviceService),
                MethodName = nameof(ParseRequestBody),
                LogLevel = LogLevel.Information,
                Message = "Parsing request body...",
            });
    }

    private void LogRequestBodyParsingSucceeded(
        ILogger logger
    )
    {
        CustomLogger.Run(logger,
            new CustomLog
            {
                ClassName = nameof(CreateDeviceService),
                MethodName = nameof(ParseRequestBody),
                LogLevel = LogLevel.Information,
                Message = "Request body is parsed successfully.",
            });
    }

    private void LogRequestBodyParsingFailed(
        ILogger logger,
        Exception e
    )
    {
        CustomLogger.Run(logger,
            new CustomLog
            {
                ClassName = nameof(CreateDeviceService),
                MethodName = nameof(ParseRequestBody),
                LogLevel = LogLevel.Error,
                Message = "Parsing request body is failed.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }

    private void LogForwardingRequestToDeviceService(
        ILogger logger
    )
    {
        CustomLogger.Run(logger,
            new CustomLog
            {
                ClassName = nameof(CreateDeviceService),
                MethodName = nameof(Run),
                LogLevel = LogLevel.Information,
                Message = "Forwarding request to DeviceService...",
            });
    }

    private void LogForwardingRequestToDeviceServiceSucceeded(
        ILogger logger
    )
    {
        CustomLogger.Run(logger,
            new CustomLog
            {
                ClassName = nameof(CreateDeviceService),
                MethodName = nameof(Run),
                LogLevel = LogLevel.Information,
                Message = "Request is forwarded to DeviceService successfully.",
            });
    }

    private void LogUnexpectedErrorOccurred(
        ILogger logger,
        Exception e
    )
    {
        CustomLogger.Run(logger,
            new CustomLog
            {
                ClassName = nameof(CreateDeviceService),
                MethodName = nameof(Run),
                LogLevel = LogLevel.Information,
                Message = "Unexpected error occurred.",
                Exception = e.Message,
                StackTrace = e.StackTrace,
            });
    }
}

