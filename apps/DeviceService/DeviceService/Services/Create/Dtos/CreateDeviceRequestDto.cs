using System;
using Newtonsoft.Json;

namespace DeviceService.Services.Create.Dtos;

public class CreateDeviceRequestDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

