﻿using System;
using Newtonsoft.Json;

namespace ArchiveService.Services.Create.Models;

public class CreateValueResponseDto
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}
