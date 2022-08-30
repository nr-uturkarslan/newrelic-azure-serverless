using System;
using Newtonsoft.Json;

namespace ProxyService.Services.Device.Create.Dtos;

public class CreateDeviceRequestDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

