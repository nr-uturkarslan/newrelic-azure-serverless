using System;
using System.Net;
using ArchiveService.Dtos;
using ArchiveService.Services.Create.Models;

namespace ArchiveService.Services.Create;

public interface ICreateFileService
{
    ResponseTemplate<CreateValueResponseDto> Run(
        CreateValueRequestDto requestDto
    );
}

public class CreateFileService : ICreateFileService
{
    public CreateFileService()
    {
    }

    public ResponseTemplate<CreateValueResponseDto> Run(
        CreateValueRequestDto requestDto
    )
    {

        var responseDto = new CreateValueResponseDto
        {
            Id = requestDto.Id,
            Name = requestDto.Name,
        };

        var response = new ResponseTemplate<CreateValueResponseDto>
        {
            Message = "Blob is successfully stored.",
            StatusCode = HttpStatusCode.Created,
            Data = responseDto,
        };

        return response;
    }
}

