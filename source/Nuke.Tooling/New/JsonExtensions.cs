using Newtonsoft.Json.Linq;

public static class JsonExtensions
{
    public static JObject ToJObject(this object obj)
    {
        return JObject.FromObject(obj);
    }
}
