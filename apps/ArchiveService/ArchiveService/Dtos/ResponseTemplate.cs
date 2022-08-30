using System.Net;
using Newtonsoft.Json;

namespace ArchiveService.Dtos;

public class ResponseTemplate<T>
{
    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("statusCode")]
    public HttpStatusCode StatusCode { get; set; }

    [JsonProperty("data")]
    public T? Data { get; set; }
}

