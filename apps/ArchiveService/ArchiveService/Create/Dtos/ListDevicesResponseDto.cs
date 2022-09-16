using System;
using Newtonsoft.Json;

namespace ArchiveService.Services.Create.Models;

public class ListDevicesResponseDto
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

