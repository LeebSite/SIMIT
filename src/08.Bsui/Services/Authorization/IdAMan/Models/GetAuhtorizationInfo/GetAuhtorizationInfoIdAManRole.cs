﻿using Newtonsoft.Json;

namespace Pertamina.SIMIT.Bsui.Services.Authorization.IdAMan.Models.GetAuhtorizationInfo;

public class GetAuhtorizationInfoIdAManRole
{
    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("permissions")]
    public string[]? Permissions { get; set; }
}
