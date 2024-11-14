using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pertamina.SIMIT.Application.Common.Extensions;

public static class GenericExtensions
{
    public static string ToPrettyJson<TRequest>(this TRequest request)
    {
        return JToken.Parse(JsonConvert.SerializeObject(request)).ToString(Formatting.Indented);
    }
}
