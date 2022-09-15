using System;
using Newtonsoft.Json;

namespace ArchiveService.Entities;

public class Device
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

