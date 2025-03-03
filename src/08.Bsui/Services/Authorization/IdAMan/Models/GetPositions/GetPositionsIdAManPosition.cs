﻿using Newtonsoft.Json;

namespace Pertamina.SIMIT.Bsui.Services.Authorization.IdAMan.Models.GetPositions;

public class GetPositionsIdAManPosition
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;
}
