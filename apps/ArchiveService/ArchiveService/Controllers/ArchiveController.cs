using Microsoft.AspNetCore.Mvc;
using ArchiveService.Commons.Logging;
using ArchiveService.Services.Create;
using ArchiveService.Services.Create.Models;

namespace ArchiveService.Controllers;

[ApiController]
[Route("[controller]")]
public class ArchiveController : ControllerBase
{
    private const string LIST_ENDPOINT_NAME = "List";

    private readonly ILogger<ArchiveController> _logger;

    private readonly IListFileService _listFileService;

    public ArchiveController(
        ILogger<ArchiveController> logger,
        IListFileService createFileService
    )
    {
        _logger = logger;
        _listFileService = createFileService;
    }

    [HttpGet(Name = LIST_ENDPOINT_NAME)]
    [Route("list")]
    public IActionResult List(
        [FromQuery] int limit = 5
    )
    {
        LogCreateEndpointIsTriggered();

        var responseDto = _listFileService.Run(limit);

        LogCreateEndpointIsFinished();

        return new OkObjectResult(responseDto);
    }

    private void LogCreateEndpointIsTriggered()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ArchiveController),
                MethodName = nameof(List),
                LogLevel = LogLevel.Information,
                Message = $"{LIST_ENDPOINT_NAME} endpoint is triggered...",
            });
    }

    private void LogCreateEndpointIsFinished()
    {
        CustomLogger.Run(_logger,
            new CustomLog
            {
                ClassName = nameof(ArchiveController),
                MethodName = nameof(List),
                LogLevel = LogLevel.Information,
                Message = $"{LIST_ENDPOINT_NAME} endpoint is finished.",
            });
    }
}

