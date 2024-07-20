using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nuke.Tooling;

public static class JsonExtensions
{
    public static JObject ToJObject(this object obj, JsonSerializer serializer = null)
    {
        serializer ??= JsonSerializer.CreateDefault();
        return JObject.FromObject(obj, serializer);
    }
}
