using System;
using System.Net;
using ArchiveService.Dtos;
using ArchiveService.Services.Create.Models;

namespace ArchiveService.Services.Create;

public interface IListFileService
{
    ResponseTemplate<ListDevicesResponseDto> Run(
        int limit
    );
}

public class ListFileService : IListFileService
{
    public ListFileService()
    {
    }

    public ResponseTemplate<ListDevicesResponseDto> Run(
        int limit
    )
    {
        var responseDto = new ListDevicesResponseDto
        {
            Id = "id",
            Name = "name",
            Description = "description"
        };

        var response = new ResponseTemplate<ListDevicesResponseDto>
        {
            Message = "Devices are successfully retrieved.",
            StatusCode = HttpStatusCode.OK,
            Data = responseDto,
        };

        return response;
    }
}

