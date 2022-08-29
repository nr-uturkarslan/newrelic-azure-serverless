using System;
using System.Net;
using Storer.Dtos;
using Storer.Services.Create.Models;

namespace Storer.Services.Create;

public interface ICreateFileService
{
    ResponseDto<CreateValueResponseDto> Run(
        CreateValueRequestDto requestDto
    );
}

public class CreateFileService : ICreateFileService
{
    public CreateFileService()
    {
    }

    public ResponseDto<CreateValueResponseDto> Run(
        CreateValueRequestDto requestDto
    )
    {

        var responseDto = new CreateValueResponseDto
        {
            Id = requestDto.Id,
            Name = requestDto.Name,
        };

        var response = new ResponseDto<CreateValueResponseDto>
        {
            Message = "Blob is successfully stored.",
            StatusCode = HttpStatusCode.Created,
            Data = responseDto,
        };

        return response;
    }
}

