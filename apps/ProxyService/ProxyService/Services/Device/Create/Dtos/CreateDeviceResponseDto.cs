using System;
using Newtonsoft.Json;

namespace ProxyService.Services.Device.Create.Dtos;

public class CreateDeviceResponseDto
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

