using System;
using Newtonsoft.Json;

namespace Storer.Services.Create.Models;

public class CreateValueRequestDto
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

