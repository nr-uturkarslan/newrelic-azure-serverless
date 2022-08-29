using Microsoft.AspNetCore.Mvc;
using Storer.Commons.Logging;
using Storer.Services.Create;
using Storer.Services.Create.Models;

namespace Storer.Controllers;

[ApiController]
[Route("[controller]")]
public class ArchiveController : ControllerBase
{
    private const string CREATE_ENDPOINT_NAME = "Create";

    private readonly ILogger<ArchiveController> _logger;

    private readonly ICreateFileService _createFileService;

    public ArchiveController(
        ILogger<ArchiveController> logger,
        ICreateFileService createFileService
    )
    {
        _logger = logger;
        _createFileService = createFileService;
    }

    [HttpPost(Name = CREATE_ENDPOINT_NAME)]
    [Route("create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateValueRequestDto requestDto
    )
    {
        LogCreateEndpointIsTriggered();

        var responseDto = _createFileService.Run(requestDto);

        LogCreateEndpointIsFinished();
        return new CreatedResult($"{responseDto.Data.Id}", responseDto);
    }

    private void LogCreateEndpointIsTriggered()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ArchiveController),
                MethodName = nameof(Create),
                LogLevel = LogLevel.Information,
                Message = $"{CREATE_ENDPOINT_NAME} endpoint is triggered...",
            });
    }

    private void LogCreateEndpointIsFinished()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ArchiveController),
                MethodName = nameof(Create),
                LogLevel = LogLevel.Information,
                Message = $"{CREATE_ENDPOINT_NAME} endpoint is finished.",
            });
    }
}

