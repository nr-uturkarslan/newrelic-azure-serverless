using System;
using Newtonsoft.Json;

namespace DeviceService.Entities;

public class Device
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("isValid")]
    public bool isValid { get; set; } = false;
}

